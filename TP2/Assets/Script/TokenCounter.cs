using UnityEngine;

public class TokenCounter : MonoBehaviour
{
    [SerializeField]
    private CollectedTokenEventSO collectedTokenEventSO;

    public int TokenCount { get; private set; }
    public int TotalCount { get; private set; }

    private void Awake()
    {
        CollectableToken[] tokens = FindObjectsByType<CollectableToken>(FindObjectsSortMode.None);

        TotalCount = tokens.Length;
        TokenCount = 0;
    }

    void Start()
    {
        collectedTokenEventSO.OnCollectedToken.AddListener(OnCollectedToken);
    }

    void OnCollectedToken(int tokenValue)
    {
        TokenCount += tokenValue;
        collectedTokenEventSO.TotalChanged(TokenCount);
    }

    private void OnValidate()
    {
        if(!collectedTokenEventSO)
        {
            Debug.LogWarning("No CollectedTokenEventSO component found on the TokenCounter GameObject.", this);
        }
    }
}
