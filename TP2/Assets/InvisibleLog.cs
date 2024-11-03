using UnityEngine;

public class InvisibleLog : MonoBehaviour
{
    public void EventInvisible(bool isInvisible)
    {
        
        Debug.Log($"Event received {isInvisible}");
    }
    
}
