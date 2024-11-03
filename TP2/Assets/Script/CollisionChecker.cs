using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField]
    Vector3 _BoxCheckHalfSize = Vector3.one;

    [SerializeField]
    Vector3 _BoxCheckOrigin = Vector3.zero;

    [SerializeField]
    LayerMask _TargetCollisionLayer;

    public bool IsInContact()
    {
        Vector3 center = transform.position + _BoxCheckOrigin;
        bool is_grounded = Physics.CheckBox(center, _BoxCheckHalfSize, Quaternion.identity, _TargetCollisionLayer);

        return is_grounded;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + _BoxCheckOrigin, 2 * _BoxCheckHalfSize);
    }
}
