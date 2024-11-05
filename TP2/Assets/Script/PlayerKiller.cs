using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerKiller : MonoBehaviour
{
    [SerializeField, Min(0f)]
    float waitTimeBeforeRestart = 1f;

    PlayerControler player;

    public void KillPlayer()
    {
        player = FindFirstObjectByType<PlayerControler>();
        player.gameObject.SetActive(false);

        StartCoroutine(RestartScene());
    }

    IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(waitTimeBeforeRestart);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
