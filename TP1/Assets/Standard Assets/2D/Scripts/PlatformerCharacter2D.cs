using System;
using UnityEngine;

#pragma warning disable 649
namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
        [SerializeField] private LayerMask m_WhatIsWall;             // A mask determining what is a wall to the character

        [SerializeField, Min(0)] private int m_MaxAerialJumps = 0;                        // Maximum number of jumps the player can perform
        [SerializeField, Min(0)] private float m_TimeAfterJumpBeforeReset = 0.1f;    // Time the player can jumpDown before reseting the jumpDown count
        [SerializeField, Min(0)] private float m_TimeBetweenJumps = 0.25f;      // Time the player can jumpDown again after another jumpDown
        [SerializeField, Min(0)] private float m_WallJumpVerticalForce = 400f; // Amount of vertical force added when the player jumps off a wall
        [SerializeField, Min(0)] private float m_WallJumpHorizontalForce = 250f; // Amount of horizontal force added when the player jumps off a wall
        [SerializeField, Min(0)] private float m_WallJumpDuration = 0.1f; // Time the player is in a wall jumpDown state
        [SerializeField, Min(0)] private float m_JumpChargingMaxForce = 400f; // Amount of maximal force for the charged jumpDown
        [SerializeField, Min(0)] private float m_JumpChargingForceRate = 100f; // Amount of force added per second for the charged jumpDown

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.

        private int m_AerialJumpCount = 0;            // Number of aerial jumps the player has performed
        private float m_TimeSinceJump = 0.0f;        // Time since the last jumpDown
        private Transform m_LeftWallCheck;          // A position marking where to check for walls on the left
        private Transform m_RightWallCheck;         // A position marking where to check for walls on the right
        const float k_WallCheckRadius = 0.15f;  // Radius of the overlap circle to determine if the player is touching a wall
        private float m_WallJumpingTime = 0.0f;    // Time since the player started wall jumping
        private bool m_IsWallJumping = false;
        private float m_JumpChargingForceProgress = 0.0f;
        private bool m_IsChargingJump = false;


        public bool Grounded => m_Grounded;


        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_LeftWallCheck = transform.Find("LeftWallCheck");
            m_RightWallCheck = transform.Find("RightWallCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }


        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_Grounded = true;
                    // Reset the aerial jumps if the player is grounded, and has not just jumped off the ground
                    if(m_TimeSinceJump >= m_TimeAfterJumpBeforeReset)
                    {
                        m_AerialJumpCount = 0;
                    }
                }
            }
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.linearVelocity.y);

            m_TimeSinceJump += Time.deltaTime;
        }


        public void Move(float move, bool crouch, bool jumpDown, bool jumpUp)
        {
            // Validate wall-jumping state
            if(m_IsWallJumping)
            {
                m_WallJumpingTime += Time.deltaTime;
                if(m_WallJumpingTime >= m_WallJumpDuration)
                {
                    m_IsWallJumping = false;
                    m_WallJumpingTime = 0.0f;
                }
            }

            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }

            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));


                // Control the speed of the character with the inputs, if we are not wall-jumping and there are inputs
                if(!m_IsWallJumping)
                {
                    m_Rigidbody2D.linearVelocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.linearVelocity.y);
                }
                

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }



            // If the player should jumpDown...
            bool canWallJumpFromLeft = IsCollidingWithLayer(m_LeftWallCheck, k_WallCheckRadius, m_WhatIsWall);
            bool canWallJumpFromRight = IsCollidingWithLayer(m_RightWallCheck, k_WallCheckRadius, m_WhatIsWall);
            bool canWallJump = canWallJumpFromLeft || canWallJumpFromRight;

            bool canAerialJump = m_AerialJumpCount < m_MaxAerialJumps && m_TimeSinceJump >= m_TimeBetweenJumps;


            // Is charging if we are crouching and pressed jumpDown and keep crouching without letting go of jump
            bool wasChargingJump = m_IsChargingJump;
            m_IsChargingJump = (m_IsChargingJump && crouch || crouch && jumpDown) && !jumpUp && m_Grounded;
            wasChargingJump = wasChargingJump && !m_IsChargingJump && m_Grounded;


            bool canJump = m_Grounded || canAerialJump || canWallJump;

            if(m_IsChargingJump)
            {
                m_JumpChargingForceProgress += Time.deltaTime * m_JumpChargingForceRate;
                m_JumpChargingForceProgress = Mathf.Min(m_JumpChargingForceProgress, m_JumpChargingMaxForce);
            }
            else if (canJump && (jumpDown || wasChargingJump))
            {
                // Adds a vertical force to the player.
                if(!m_Grounded && !canWallJump)
                {
                    m_AerialJumpCount++;
                }

                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_TimeSinceJump = 0.0f;

                // Reset the vertical velocity before applying the jumpDown force to get a consistent jumpDown
                m_Rigidbody2D.linearVelocityY = 0.0f;

                bool willWallJump = canWallJump && !m_Grounded;

                if(willWallJump)
                {
                    float horizontalForce = m_WallJumpHorizontalForce;
                    if(!m_FacingRight)
                    {
                        horizontalForce *= -1;
                    }
                    m_Rigidbody2D.linearVelocityX = 0.0f;
                    if(canWallJumpFromLeft)
                    {
                        m_Rigidbody2D.AddForce(new Vector2(horizontalForce, m_WallJumpVerticalForce));
                    }
                    else
                    {
                        m_Rigidbody2D.AddForce(new Vector2(-horizontalForce, m_WallJumpVerticalForce));
                    }

                    m_IsWallJumping = true;
                }
                else // Jumping off the ground or in the air
                {
                    float jumpForce = m_JumpForce;
                    if(wasChargingJump)
                    {
                        jumpForce += m_JumpChargingForceProgress;
                        m_JumpChargingForceProgress = 0.0f;
                    }
                    m_Rigidbody2D.AddForce(new Vector2(0f, jumpForce));
                }
            }
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }



        private bool IsCollidingWithLayer(Transform point, float checkRadius, LayerMask layer)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, checkRadius, layer);
            for(int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i].gameObject != gameObject)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
