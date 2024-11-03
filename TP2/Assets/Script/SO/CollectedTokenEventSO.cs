using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CollectedTokenEvent.asset", menuName = "Scriptable Objects/CollectedTokenEvent")]
public class CollectedTokenEventSO : ScriptableObject
{
    [System.NonSerialized]
    public UnityEvent<int> OnCollectedToken;

    [System.NonSerialized]
    public UnityEvent<int> OnTotalChanged;

    public void Increase(int tokenValue)
    {
        OnCollectedToken?.Invoke(tokenValue);
    }

    public void TotalChanged(int totalToken)
    {
        OnTotalChanged?.Invoke(totalToken);
    }

    private void OnEnable()
    {
        if(OnCollectedToken == null)
        {
            OnCollectedToken = new UnityEvent<int>();
        }
        if(OnTotalChanged == null)
        {
            OnTotalChanged = new UnityEvent<int>();
        }
    }
}
