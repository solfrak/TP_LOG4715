using System;
using UnityEngine;

public class PowerUpPicker : MonoBehaviour
{

    public PowerUpSO powerup;
    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.transform.gameObject;
        powerup.Apply(gameObject);
    }
}
