using UnityEngine;
using UnityEngine.Events;

public class laser : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public UnityEvent trigger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                //playerHealth.TakeDamage(playerHealth.maxHealth);
                trigger.Invoke();
            }
        }
    }
}
