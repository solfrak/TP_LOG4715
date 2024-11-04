
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpSO", menuName = "PowerUp/MovementBuff")]
public class MouvementBuff : PowerUpSO
{
    public float jumpForceAdded;
    public float moveSpeedAdded;
    
    public override void Apply(GameObject target)
    {
        var controller = target.GetComponentInChildren<PlayerControler>();

        if (controller)
        {
            controller.moveSpeed += moveSpeedAdded;
            controller.jumpForce += jumpForceAdded;
        }
    }
}