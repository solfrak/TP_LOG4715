using UnityEngine;

public class EndManager : MonoBehaviour
{
    [SerializeField]
    CollisionChecker collisionChecker;

    [SerializeField]
    AudioClip endWinSFX;

    [SerializeField]
    Canvas endScreenCanvas;

    bool isOver = false;

    // Update is called once per frame
    void Update()
    {
        if(collisionChecker.IsInContact() && !isOver)
        {
            isOver = true;
            FindAnyObjectByType<PlayerControler>()?.gameObject.SetActive(false);
            FindAnyObjectByType<AudioManager>()?.PlaySFX(endWinSFX);
            endScreenCanvas?.gameObject.SetActive(true);
        }
    }
}
