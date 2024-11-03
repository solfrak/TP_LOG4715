using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    public Vector3 rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
