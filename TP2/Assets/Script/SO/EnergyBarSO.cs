using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnergyBarSO", menuName = "Scriptable Objects/EnergyBarSO")]
public class EnergyBarSO : ScriptableObject
{
    public float MaxEnergy;
    public float MinEnergy;

    public float EnergyRestoreSpeed;

    public float CurrentEnergy;

    public bool HasEnoughEnergy(float AddedValue)
    {
        if (CurrentEnergy + AddedValue > MinEnergy)
            return true;

        return false;
    }

    private void OnEnable()
    {
        CurrentEnergy = (MaxEnergy - MinEnergy) / 2;
    }

    public void UpdateEnergy(float value)
    {
        CurrentEnergy += value;

        if (CurrentEnergy < MinEnergy)
            CurrentEnergy = MinEnergy;

        if (CurrentEnergy > MaxEnergy)
            CurrentEnergy = MaxEnergy;
    }
}
