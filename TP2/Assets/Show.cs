using System;
using UnityEngine;
using UnityEngine.UI;

public class Show : MonoBehaviour
{
    public Toggle isInRange;
    public Toggle isDetected;

    public FieldOfView FieldOfView;

    private void Update()
    {
        isDetected.isOn = FieldOfView.IsDetected;
        isInRange.isOn = FieldOfView.IsInRange;
    }
}
