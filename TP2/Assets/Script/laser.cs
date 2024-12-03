using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class laser : MonoBehaviour
{
    [SerializeField]
    bool isLaserActive;

    [SerializeField]
    AudioClip laserActivatesSFX;

    bool playSoundWhenActivating = false;

    public UnityEvent HitEvent;

    private void Start()
    {
        SetLaserState(isLaserActive);

        // Avoid playing the sound when the laser inits
        playSoundWhenActivating = true;
    }

    public bool IsLaserActive {
        get { return isLaserActive; }
        private set { isLaserActive = value; }
    }


    public void SetLaserState(bool state)
    {
        IsLaserActive = state;
        gameObject.SetActive(isLaserActive);

        if(state && playSoundWhenActivating)
        {
            FindAnyObjectByType<AudioManager>()?.PlaySFX(laserActivatesSFX);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HitEvent?.Invoke();
        }
    }
}
