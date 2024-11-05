using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class laser : MonoBehaviour
{
    [SerializeField]
    bool isLaserActive;


    public UnityEvent trigger;

    private void Start()
    {
        SetLaserState(isLaserActive);
    }

    public bool IsLaserActive {
        get { return isLaserActive; }
        private set { isLaserActive = value; }
    }


    public void SetLaserState(bool state)
    {
        IsLaserActive = state;
        gameObject.SetActive(isLaserActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trigger?.Invoke();
        }
    }
}
