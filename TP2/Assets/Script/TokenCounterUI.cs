using UnityEngine;
using TMPro;

public class TokenCounterUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    private TokenCounter tokenCounter;

    [SerializeField]
    private CollectedTokenEventSO collectedTokenEventSO;

    void Start()
    {
        UpdateText(tokenCounter.TokenCount);
        collectedTokenEventSO.OnTotalChanged.AddListener(UpdateText);
    }

    void UpdateText(int total)
    {
        text.text = $"Collected: {total}/{tokenCounter.TotalCount}";
    }

    private void OnValidate()
    {
        if (!text)
        {
            Debug.LogWarning("No TextMeshProUGUI component found on the TokenCounterUI GameObject.", this);
        }
        if(!tokenCounter)
        {
            Debug.LogWarning("No TokenCounter component found on the TokenCounterUI GameObject.", this);
        }
        if(!collectedTokenEventSO)
        {
            Debug.LogWarning("No CollectedTokenEventSO component found on the TokenCounterUI GameObject.", this);
        }
    }
}
