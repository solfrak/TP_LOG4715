using System;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    public EnergyBarSO _energy;

    private void FixedUpdate()
    {
        _energy.UpdateEnergy(_energy.EnergyRestoreSpeed * Time.deltaTime);
    }
}
