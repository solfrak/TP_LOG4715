
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PowerUpSO", menuName = "PowerUp/SpeedBuff")]
public class SpeedBuff : PowerUpSO
{
    [FormerlySerializedAs("jumpForceAdded")] public float speeForceAdded;
    
    public override void Apply(GameObject target)
    {
        var controller = target.GetComponentInChildren<PlayerControler>();

        if (controller)
        {
            controller.moveSpeed += speeForceAdded;
        }
    }
}