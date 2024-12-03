using UnityEngine;

public class CollectableToken : MonoBehaviour
{
    [SerializeField]
    LayerMask layerMaskCollidesWith;

    [SerializeField]
    CollectedTokenEventSO collectedTokenEventSO;

    [SerializeField]
    AudioClip collectedSound;

    bool collected = false;
    
    void Start()
    {
        collected = false;
    }

    void OnTriggerEnter(Collider other)
    {
        bool collidesWithLayer = (layerMaskCollidesWith & (1 << other.gameObject.layer)) != 0;
        if(collidesWithLayer)
        {
            Collect();
        }
    }

    void Collect()
    {
        if(collected)
        {
            return;
        }

        collected = true;
        collectedTokenEventSO.Increase(1);
        FindAnyObjectByType<AudioManager>()?.PlaySFX(collectedSound);
        Destroy(gameObject);
    }


    private void OnValidate()
    {
        if(layerMaskCollidesWith == 0)
        {
            Debug.LogWarning("layerMaskCollidesWith is set to Nothing", this);
        }
        if(collectedTokenEventSO == null)
        {
            Debug.LogWarning("collectedTokenEventSO is not set", this);
        }
    }
}
