using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerKiller : MonoBehaviour
{
    [SerializeField, Min(0f)]
    float waitTimeBeforeRestart = 2f;

    [SerializeField]
    AudioClip playerDeathSFX;

    PlayerControler player;

    bool isPlayerDead = false;

    private void Start()
    {
        isPlayerDead = false;
    }

    public void KillPlayer()
    {
        if(isPlayerDead)
        {
            return;
        }
        isPlayerDead = true;
        player = FindFirstObjectByType<PlayerControler>();
        player.gameObject.SetActive(false);

        FindAnyObjectByType<AudioManager>()?.PlaySFX(playerDeathSFX);

        StartCoroutine(RestartScene());
    }

    IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(waitTimeBeforeRestart);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
