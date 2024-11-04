using UnityEngine;

// [CreateAssetMenu(fileName = "PowerUpSO", menuName = "Scriptable Objects/PowerUpSO")]
public abstract class PowerUpSO : ScriptableObject
{
    public int Cost;
    public abstract void Apply(GameObject target);
}
