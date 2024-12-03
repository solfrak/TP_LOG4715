using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class laser : MonoBehaviour
{
    [SerializeField]
    bool isLaserActive;

    [SerializeField]
    float timeBeforeStopping = 0f;

    [SerializeField]
    AudioClip laserActivatesSFX;

    bool playSoundWhenActivating = false;

    bool isLaserClosing = false;

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
        if(!IsLaserActive && state && playSoundWhenActivating)
        {
            FindAnyObjectByType<AudioManager>()?.PlaySFX(laserActivatesSFX);
        }
        if(state)
        {
            isLaserClosing = false;
        }
        IsLaserActive = state;
        gameObject.SetActive(isLaserActive);
    }

    public void SetLaserByDetection(GameObject obj, bool state)
    {
        if(state)
        {
            SetLaserState(state);
        }
        else
        {
            isLaserClosing = true;
            StartCoroutine(SetStateAfterTime(state, timeBeforeStopping));
        }
    }

    private IEnumerator SetStateAfterTime(bool state, float time)
    {
        yield return new WaitForSeconds(time);
        bool isLaserStillClosing = isLaserClosing && !state;
        if(isLaserClosing || state)
        {
            SetLaserState(state);
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
