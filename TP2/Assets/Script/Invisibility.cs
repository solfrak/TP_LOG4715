using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Invisibility : MonoBehaviour
{
    public float InvisibleTime;
    public float InvisibleRestoreTime;
    public UnityEvent<bool> isInvisibleEvent;

    private InputAction invisibilityAction;

    private bool IsActionTrigger
    {
        get { return invisibilityAction.ReadValue<float>() > 0; }
    }

    private bool IsInvisible = false;
    private bool CanBecomeInvisible = true;

    void Start()
    {
        invisibilityAction = InputSystem.actions.FindAction("Invisible");
    }

    // Update is called once per frame
    void Update()
    {
        if (CanBecomeInvisible && !IsInvisible && IsActionTrigger)
        {
            //Trigger Invisibility
            StartInvisibility();
        }
    }

    void StartInvisibility()
    {
        Debug.Log("Become invisible");
        //Change some stuff
        IsInvisible = true;
        CanBecomeInvisible = false;
        isInvisibleEvent?.Invoke(true);
        StartCoroutine(Timer(InvisibleTime, StopInvisibility));
    }

    void StopInvisibility()
    {
        Debug.Log("Become visible");
        IsInvisible = false;
        isInvisibleEvent?.Invoke(false);
        StartCoroutine(Timer(InvisibleRestoreTime, RestoreAbility));
    }

    void RestoreAbility()
    {
        Debug.Log("Can become invisible");
        CanBecomeInvisible = true;
    }

    IEnumerator Timer(float time, Action func)
    {
        yield return new WaitForSeconds(time);
        func();

    }


}
