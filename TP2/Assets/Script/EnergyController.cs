using System;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    public EnergyBarSO _energy;

    [SerializeField]
    CollectedTokenEventSO collectedTokenEvent;

    [SerializeField]
    int tokenStaminaGain = 25;

    private void Start()
    {
        collectedTokenEvent.OnCollectedToken.AddListener(OnTokenPickup);
    }

    private void FixedUpdate()
    {
        _energy.UpdateEnergy(_energy.EnergyRestoreSpeed * Time.deltaTime);
    }

    private void OnTokenPickup(int total)
    {
        _energy.UpdateEnergy(tokenStaminaGain);
    }
}
