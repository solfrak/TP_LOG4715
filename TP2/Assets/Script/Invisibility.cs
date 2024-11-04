using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Invisibility : MonoBehaviour
{
    public float InvisibleTime;
    public float InvisibleRestoreTime;
    public UnityEvent isInvisibleEvent;
    public UnityEvent isVisibleEvent;
    
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
            //Trigger Invisibility
            StartInvisibility();
        }
    }

    void StartInvisibility()
    {
        Debug.Log("Become invisible");
        //Change some stuff
        m_IsInvisible = true;
        m_CanBecomeInvisible = false;
        isInvisibleEvent?.Invoke();
        StartCoroutine(Timer(InvisibleTime, StopInvisibility));
    }

    void StopInvisibility()
    {
        Debug.Log("Become visible");
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
