using System;
using UnityEngine;
using UnityEngine.UI;

public class Show : MonoBehaviour
{
    public Toggle isInRange;
    public Toggle isDetected;
    public Toggle isInvisible;

    public FieldOfView FieldOfView;
    public Invisibility Invisibility;

    private void Update()
    {
        isDetected.isOn = FieldOfView && FieldOfView.IsDetected;
        isInRange.isOn = FieldOfView && FieldOfView.IsInRange;
        isInvisible.isOn = Invisibility && Invisibility.IsInvisible;
    }
}
