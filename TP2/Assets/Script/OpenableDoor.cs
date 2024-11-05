using System.Collections;
using UnityEngine;

public class OpenableDoor : MonoBehaviour
{
    bool opened = false;
    Vector3 startingPosition;

    [SerializeField, Min(0f)]
    float openingSpeed = 1f;

    [SerializeField]
    Vector3 openedPositionRelative;

    private void Start()
    {
        startingPosition = transform.position;
    }

    public void Open()
    {
        if(opened)
        {
            return;
        }
        opened = true;
        StartCoroutine(MoveCoroutine(transform.position + openedPositionRelative));
    }

    public void Close()
    {
        if(!opened)
        {
            return;
        }
        opened = false;
        StartCoroutine(MoveCoroutine(startingPosition));
    }

    IEnumerator MoveCoroutine(Vector3 destination)
    {
        const float tolerance = 0.01f;
        while((transform.position - destination).magnitude > tolerance)
        {
            Vector3 direction = (destination - transform.position).normalized;
            transform.Translate(Time.deltaTime * openingSpeed * direction, Space.World);
            yield return null;
        }

        transform.position = destination;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if(!meshFilter)
        {
            return;
        }
        Gizmos.DrawWireMesh(meshFilter.sharedMesh, transform.position + openedPositionRelative, transform.rotation, transform.lossyScale);
    }
}
