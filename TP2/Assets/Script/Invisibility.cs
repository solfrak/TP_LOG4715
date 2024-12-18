using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Invisibility : MonoBehaviour
{
    public float InvisibleTime;
    public float InvisibleRestoreTime;
    public UnityEvent isInvisibleEvent;
    public UnityEvent isVisibleEvent;

    // public SkinnedMeshRenderer m_MeshRenderer;

    public List<SkinnedMeshRenderer> m_MeshRenderers;
    public List<Material> m_Materials;
    
    public Material head_material;
    public Material body_material;

    public AudioSource m_AudioSource;
    public AudioClip m_SFXInvisible;
    public AudioClip m_SFXVisible;
    public AudioClip m_SFXNotEnoughEnergy;

    public EnergyBarSO EnergyBarSo;
    public float InvisibilityEnergyCost;
    
    public bool IsInvisible
    {
        get { return m_IsInvisible; }
    }

    private InputAction invisibilityAction;

    private bool IsActionTrigger
    {
        get { return invisibilityAction.ReadValue<float>() > 0; }
    }

    [SerializeField]
    private bool m_IsInvisible = false;
    private bool m_CanBecomeInvisible = true;

    void Start()
    {
        invisibilityAction = InputSystem.actions.FindAction("Invisible");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CanBecomeInvisible && !m_IsInvisible && IsActionTrigger)
        {
            if (EnergyBarSo.HasEnoughEnergy(InvisibilityEnergyCost))
            {
                EnergyBarSo.UpdateEnergy(InvisibilityEnergyCost);
                //Trigger Invisibility
                StartInvisibility();
            }
            else
            {
                m_AudioSource.PlayOneShot(m_SFXNotEnoughEnergy);
            }
        }
        else
        {
            if (!m_CanBecomeInvisible && Input.GetKeyDown(KeyCode.E))
            {
                m_AudioSource.PlayOneShot(m_SFXNotEnoughEnergy);
            }
        }
    }

    void StartInvisibility()
    {

        // Color color = m_MeshRenderer.material.color;


        for (int i = 0; i < m_MeshRenderers.Count; i++)
        {
            m_MeshRenderers[i].materials[0].color = Color.gray;
        }
        // m_MeshRenderer.materials[0].color = Color.gray;
        // m_MeshRenderer.materials[1].color = Color.gray;
        //Change some stuff
        m_IsInvisible = true;
        m_CanBecomeInvisible = false;
        isInvisibleEvent?.Invoke();
        m_AudioSource.PlayOneShot(m_SFXInvisible);
        StartCoroutine(Timer(InvisibleTime, StopInvisibility));
    }

    void StopInvisibility()
    {
        Debug.Log("Become visible");
        for (int i = 0; i < m_MeshRenderers.Count; i++)
        {
            m_MeshRenderers[i].materials[0].color = m_Materials[i].color;
        }
        // m_MeshRenderer.materials[0].color = body_material.color;
        // m_MeshRenderer.materials[1].color = head_material.color;
        m_IsInvisible = false;
       
        isVisibleEvent?.Invoke();
        StartCoroutine(Timer(InvisibleRestoreTime, RestoreAbility));
    }

    void RestoreAbility()
    {
        Debug.Log("Can become invisible");
        m_CanBecomeInvisible = true;
    }

    IEnumerator Timer(float time, Action func)
    {
        yield return new WaitForSeconds(time);
        func();

    }


}
