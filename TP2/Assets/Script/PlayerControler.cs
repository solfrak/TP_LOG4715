using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerControler : MonoBehaviour
{
    enum SFX_Sound
    {
        Jump,
        DoubleJump,
        WallJump,
        HitGround,
    }
    // Constants
    private static readonly Vector3 FlipRotation = new Vector3(0, 180, 0);
    
    public AudioSource m_AudioSource;
    public List<AudioClip> AudioClips = new List<AudioClip>();

    // Variables
    bool _Grounded { get; set; }
    private bool _CanHitGround { get; set; }
    bool _Flipped { get; set; }
    bool _IsInContactWithLeftWall { get; set; }
    bool _IsInContactWithRightWall { get; set; }
    bool _IsInContactWithLeftWallNext { get; set; }
    bool _IsInContactWithRightWallNext { get; set; }
    int _NbAerialJumpsUsed { get; set; }
    float _TotalChargedJumpForce { get; set; }
    float _TimeSinceLastAerialJump { get; set; }
    float _TimeSinceWallJumpStart { get; set; }
    float _TimeJumpHeld { get; set; }
    Animator _Anim { get; set; }
    Rigidbody _Rb { get; set; }
    CollisionChecker _GroundChecker { get; set; }

    // Exposed Variables
    [Header("Energy")]
    [SerializeField]
    private EnergyBarSO energyBarSo;

    [SerializeField] private float CostJump;
    [SerializeField] private float DoubleJump;
    [SerializeField] private float WallJump;
    

    [Header("Movement")]
    
    
    [SerializeField, Min(0f)]
    public float moveSpeed = 5.0f;

    [Header("Jump")]
    [SerializeField, Min(0f)]
    public float jumpForce = 5f;

    [SerializeField, Min(0f)]
    float chargedJumpDuration = 1f;

    [SerializeField, Min(0f)]
    float chargingJumpForcePerSecond = 5f;

    [Header("Aerial Jump")]
    [SerializeField, Min(0f)]
    float aerialJumpForce = 10f;

    [SerializeField, Min(0)]
    public int nbAerialJumpsMax = 1;

    [SerializeField, Min(0f)]
    float timeBetweenAerialJumpsMin = 0.25f;

    [Header("Wall Jump")]
    [SerializeField, Min(0f)]
    float wallJumpVerticalForce = 10f;

    [SerializeField, Min(0f)]
    float wallJumpHorizontalForce = 10f;

    [SerializeField, Min(0f)]
    float wallJumpDuration = 0.20f;

    [Header("Misc.")]
    [SerializeField]
    LayerMask whatIsGround;

    // Awake se produit avait le Start. Il peut être bien de régler les références dans cette section.
    void Awake()
    {
        _Anim = GetComponent<Animator>();
        _Rb = GetComponent<Rigidbody>();
        _GroundChecker = GetComponent<CollisionChecker>();
    }

    // Utile pour régler des valeurs aux objets
    void Start()
    {
        _Grounded = false;
        _CanHitGround = false;
        _IsInContactWithLeftWall = false;
        _IsInContactWithRightWall = false;
        _IsInContactWithRightWallNext = false;
        _IsInContactWithRightWallNext = false;
        _TotalChargedJumpForce = 0f;
        _Flipped = false;
        _NbAerialJumpsUsed = 0;
        _TimeSinceLastAerialJump = timeBetweenAerialJumpsMin;
        _TimeSinceWallJumpStart = 999f;
    }

    // Check player inputs
    void Update()
    {
        _TimeSinceLastAerialJump += Time.deltaTime;
        _TimeSinceWallJumpStart += Time.deltaTime;
        var horizontal = Input.GetAxis("Horizontal") * moveSpeed;
        HorizontalMove(horizontal);
        FlipCharacter(horizontal);
        CheckJump();
    }

    void FixedUpdate()
    {
        CheckState();
    }

    // Handles horizontal movement
    void HorizontalMove(float horizontal)
    {
        if(_TimeSinceWallJumpStart <= wallJumpDuration)
        {
            return;
        }    

        if(_IsInContactWithLeftWall && horizontal < 0)
        {
            horizontal = 0;
        }
        else if(_IsInContactWithRightWall && horizontal > 0)
        {
            horizontal = 0;
        }

        _Rb.linearVelocity = new Vector3(_Rb.linearVelocity.x, _Rb.linearVelocity.y, horizontal);
        _Anim.SetFloat("MoveSpeed", Mathf.Abs(horizontal));
    }

    // Handles the Jumps and the jump animation
    void CheckJump()
    {
        // Additionnal jump force
        float maxAdditionalJumpForce = chargingJumpForcePerSecond * chargedJumpDuration;
        if(!_Grounded && _TotalChargedJumpForce < maxAdditionalJumpForce)
        {
            if(Input.GetButton("Jump"))
            {
                float additionalForce = chargingJumpForcePerSecond * Time.deltaTime;
                _Rb.AddForce(new Vector3(0, chargingJumpForcePerSecond * Time.deltaTime, 0f), ForceMode.VelocityChange);
                _TotalChargedJumpForce += additionalForce;
            }
            else // If jump is released, dont let the player jump higher
            {
                _TotalChargedJumpForce = maxAdditionalJumpForce;
            }
        }

        if(Input.GetButtonDown("Jump") )
        {
            // Grounded jump
            if(_Grounded)
            {
                _Rb.AddForce(new Vector3(0, jumpForce, 0f), ForceMode.Impulse);
                _Grounded = false;
                _Anim.SetBool("Grounded", false);
                _Anim.SetBool("Jump", true);
                m_AudioSource.PlayOneShot(AudioClips[(int)SFX_Sound.Jump]);
            }
            // Wall jump
            else if(_IsInContactWithLeftWall || _IsInContactWithRightWall && (energyBarSo  == null || energyBarSo.HasEnoughEnergy(CostJump)))
            {
                float signedWallJumpHorizontalForce = _IsInContactWithLeftWall ? wallJumpHorizontalForce : -wallJumpHorizontalForce;

                // Reset velocity to make the aerial jump consistent
                _Rb.linearVelocity = new Vector3(_Rb.linearVelocity.x, 0f, 0f);
                _Rb.AddForce(new Vector3(0f, wallJumpVerticalForce, signedWallJumpHorizontalForce), ForceMode.Impulse);
                m_AudioSource.PlayOneShot(AudioClips[(int)SFX_Sound.WallJump]);

                _TimeSinceWallJumpStart = 0f;
                _Grounded = false;
                _Anim.SetBool("Grounded", false);
                _Anim.SetBool("Jump", true);
                energyBarSo?.UpdateEnergy(CostJump);
            }
            // Aerial Jumps
            else if(_NbAerialJumpsUsed < nbAerialJumpsMax
                && _TimeSinceLastAerialJump > timeBetweenAerialJumpsMin && (energyBarSo  == null || energyBarSo.HasEnoughEnergy(CostJump)))
            {
                // Reset vertical velocity to make the aerial jump consistent
                _Rb.linearVelocity = new Vector3(_Rb.linearVelocity.x, 0f, _Rb.linearVelocity.z);
                _Rb.AddForce(new Vector3(0f, aerialJumpForce, 0f), ForceMode.Impulse);
                m_AudioSource.PlayOneShot(AudioClips[(int)SFX_Sound.DoubleJump]);

                _Grounded = false;
                _Anim.SetBool("Grounded", false);
                _Anim.SetBool("Jump", true);
                

                _NbAerialJumpsUsed++;
                _TimeSinceLastAerialJump = 0f;
                energyBarSo?.UpdateEnergy(CostJump);
            }
        }

        if(_Grounded)
        {
            _TotalChargedJumpForce = 0f;
        }

        if (_CanHitGround && _GroundChecker.IsInContact())
        {
            _CanHitGround = false;
            m_AudioSource.PlayOneShot(AudioClips[(int)SFX_Sound.HitGround]);
        }
        else if(!_GroundChecker.IsInContact())
        {
            _CanHitGround = true;
        }
        
        
        
    }

    void CheckState()
    {
        _IsInContactWithLeftWall = _IsInContactWithLeftWallNext;
        _IsInContactWithRightWall = _IsInContactWithRightWallNext;
        _Grounded = _GroundChecker.IsInContact();

        if(_Grounded)
        {
            _NbAerialJumpsUsed = 0;
            _TimeSinceLastAerialJump = timeBetweenAerialJumpsMin;
        }
        _Anim.SetBool("Grounded", _Grounded);

        _IsInContactWithLeftWallNext = false;
        _IsInContactWithRightWallNext = false;
    }


    // Handles the character flip and the camera
    void FlipCharacter(float horizontal)
    {
        if (horizontal < 0 && !_Flipped)
        {
            _Flipped = true;
            transform.Rotate(FlipRotation);
        }
        else if (horizontal > 0 && _Flipped)
        {
            _Flipped = false;
            transform.Rotate(-FlipRotation);
        }
    }

    private void OnCollisionStay(Collision coll)
    {
        bool isInContactWithGround = (whatIsGround & (1 << coll.gameObject.layer)) != 0;
        if(coll.contacts.Length == 0 || !isInContactWithGround)
        {
            return;
        }

        const float threshold = 0.95f;
        foreach(ContactPoint contact in coll.contacts)
        {
            if(Vector3.Dot(contact.normal.normalized, Vector3.forward) > threshold)
            {
                _IsInContactWithLeftWallNext = true;
            }
            else if(Vector3.Dot(contact.normal.normalized, Vector3.back) > threshold)
            {
                _IsInContactWithRightWallNext = true;
            }
        }
    }

    private void OnValidate()
    {
        if(whatIsGround == 0)
        {
            Debug.LogError($"Please set the whatIsGround variable in the inspector", this);
        }
    }
}