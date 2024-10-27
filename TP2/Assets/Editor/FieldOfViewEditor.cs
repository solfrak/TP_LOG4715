using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.right, Vector3.forward, 360, fow.ViewRadius);
        
        Vector3 viewAngleA = fow.DirFromAngle(-fow.ViewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.ViewAngle / 2, false);

        
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.ViewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.ViewRadius);
    }
}
