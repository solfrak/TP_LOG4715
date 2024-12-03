using System.Collections;
using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class FieldOfView : MonoBehaviour
{
    public float ViewRadius;

    public MeshRenderer CurrentMeshRenderer;
    public Material DetectedMaterial;
    public Material NotDetectedMaterial;

    [Range(0, 360)] public float ViewAngle;

    public float TimeBeforeDetecting;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public UnityEvent<GameObject, bool> DetectionEvent;

    public bool IsInRange
    {
        get { return isInRange; }
    }

    public bool IsDetected
    {
        get { return isDetected; }
        private set
        {
            CurrentMeshRenderer.material.color = value ? DetectedMaterial.color : NotDetectedMaterial.color;
            isDetected = value;
        }
    }


    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;
    
    public MeshFilter viewMeshFilter;

    private bool isInRange = false;
    private GameObject detectedPlayer;
    private float timeDetectedPlayer;

    Mesh viewMesh;
    private void Update()
    {
        FindVisibleTarget();
        DrawFieldofView();
    }

    private void Start()
    {
        viewMesh = new Mesh ();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

    }

    private void LateUpdate()
    {
        DrawFieldofView();
    }

    void FindVisibleTarget()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, targetMask);

        if (targetsInViewRadius.Length == 0)
        {
            isInRange = false;
            IsDetected = false;
            return;
        }
        
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            Vector3 transformFoward = transform.forward;
            GameObject character = GetParent(targetsInViewRadius[i].gameObject);
            Invisibility? invisibility = character.GetComponent<Invisibility>();
            bool isPlayerInvisible = invisibility && invisibility.IsInvisible;
            if (!isPlayerInvisible && Vector3.Angle(transformFoward, dirToTarget) < ViewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    timeDetectedPlayer += Time.deltaTime;
                    CurrentMeshRenderer.material.color = Color.Lerp(NotDetectedMaterial.color, DetectedMaterial.color, timeDetectedPlayer / TimeBeforeDetecting);
                    if(timeDetectedPlayer >= TimeBeforeDetecting && !IsDetected)
                    {
                        IsDetected = true;
                        DetectionEvent?.Invoke(targetsInViewRadius[i].gameObject, IsDetected);
                    }
                }
            }
            else
            {
                // If was it was detected
                if(IsDetected)
                {
                    DetectionEvent?.Invoke(targetsInViewRadius[i].gameObject, false);
                }

                IsDetected = false;
                isInRange = false;
                timeDetectedPlayer = 0f;
            }
        }
    }

    private GameObject GetParent(GameObject o)
    {
        GameObject current = o;
        while (current.transform.parent != null)
        {
            current = current.transform.parent.gameObject;
        }

        return current;
    }

    private bool isDetected = false;
    

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal)
        {
            angleInDegrees -= GetEulerAngleXAxis(transform.eulerAngles);
        }
        return new Vector3(0, Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    void DrawFieldofView()
    {
        int stepCount = Mathf.RoundToInt(ViewAngle * meshResolution);
        float stepAngleSize = ViewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3> ();
        ViewCastInfo oldViewCast = new ViewCastInfo ();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = 360 - (GetEulerAngleXAxis(transform.eulerAngles) - ViewAngle / 2 + stepAngleSize * i);
            ViewCastInfo newViewCast = ViewCast (angle);
            Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * ViewRadius, Color.red);
            if (i > 0) {
                bool edgeDstThresholdExceeded = Mathf.Abs (oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded)) {
                    EdgeInfo edge = FindEdge (oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero) {
                        viewPoints.Add (edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero) {
                        viewPoints.Add (edge.pointB);
                    }
                }

            }
            
            viewPoints.Add (newViewCast.point);
            oldViewCast = newViewCast;
        }
        
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount-2) * 3];

        vertices [0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++) {
            vertices [i + 1] = transform.InverseTransformPoint(viewPoints [i]);

            if (i < vertexCount - 2) {
                triangles [i * 3] = 0;
                triangles [i * 3 + 1] = i + 1;
                triangles [i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear ();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals ();
    }
    ViewCastInfo ViewCast(float globalAngle) {
        Vector3 dir = DirFromAngle (globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast (transform.position, dir, out hit, ViewRadius, obstacleMask)) {
            return new ViewCastInfo (true, hit.point, hit.distance, globalAngle);
        } else {
            return new ViewCastInfo (false, transform.position + dir * ViewRadius, ViewRadius, globalAngle);
        }
    }
    
    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++) {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast (angle);

            bool edgeDstThresholdExceeded = Mathf.Abs (minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded) {
                minAngle = angle;
                minPoint = newViewCast.point;
            } else {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo (minPoint, maxPoint);
    }
 

    float GetEulerAngleXAxis(Vector3 eulerAngle)
    {
        return eulerAngle.y == 180.0f ? 180 - eulerAngle.x : eulerAngle.x;
    }

    public struct ViewCastInfo {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle) {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }
    
    public struct EdgeInfo {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB) {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
