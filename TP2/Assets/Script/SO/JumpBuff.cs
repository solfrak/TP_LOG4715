
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpSO", menuName = "PowerUp/JumpBuff")]
public class JumpBuff : PowerUpSO
{
    public float jumpForceAdded;
    
    public override void Apply(GameObject target)
    {
        var controller = target.GetComponentInChildren<PlayerControler>();

        if (controller)
        {
            controller.jumpForce += jumpForceAdded;
        }
    }
}