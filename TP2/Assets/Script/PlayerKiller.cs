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

    public void KillPlayer()
    {
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
