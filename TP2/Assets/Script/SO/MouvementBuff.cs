
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpSO", menuName = "PowerUp/MovementBuff")]
public class MouvementBuff : PowerUpSO
{
    public float jumpForceAdded;
    public float moveSpeedAdded;
    
    public override void Apply(GameObject target)
    {
        var controller = target.GetComponent<PlayerControler>();

        if (controller)
        {
            controller.JumpForce += jumpForceAdded;
            controller.MoveSpeed += moveSpeedAdded;
        }
    }
}