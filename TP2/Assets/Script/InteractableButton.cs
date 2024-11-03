using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CollisionChecker))]
public class InteractableButton : MonoBehaviour
{
    bool pressed = false;
    CollisionChecker collisionChecker;

    public UnityEvent OnPressed;

    private void Awake()
    {
        collisionChecker = GetComponent<CollisionChecker>();
    }

    private void FixedUpdate()
    {
        if(!pressed && collisionChecker.IsInContact())
        {
            Press();
        }
    }

    void Press()
    {
        if(pressed)
        {
            return;
        }
        OnPressed?.Invoke();
    }
}
