using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/CameraDirect")]
public class CameraDirectData : ScriptableObject
{
    public List<CameraDirectType> typeList = new();

    public float directTime;

    // Shake
    public float shakeAmplitude = 0;
    public float shakeFrequency = 0;
    public AnimationCurve shakeCurve = AnimationCurve.Linear(0, 1, 1, 1);

    // Zoom
    public AnimationCurve zoomCurve = AnimationCurve.Linear(0, 1, 1, 1);

    // Rotate
    public Vector3 rotateValue = Vector3.zero;
    public AnimationCurve rotateCurve = AnimationCurve.Linear(0, 1, 1, 1);

    // Position
    public Vector3 positionValue = Vector3.zero;
    public AnimationCurve positionCurve = AnimationCurve.Linear(0, 1, 1, 1);
}

public enum CameraDirectType
{
    Shake,
    Zoom,
    Rotate,
    Position,
}