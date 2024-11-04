using System;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public CollectedTokenEventSO EventSo;
    public PowerUpSO powerup;

    public void OnEnable()
    {
        EventSo.OnCollectedToken.AddListener(AddPowerUp);
    }

    void AddPowerUp(int dump)
    {
        var player = GameObject.FindWithTag("Player");
        powerup.Apply(player);
    }
}
