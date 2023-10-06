using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFOVLimit : MonoBehaviour
{
    [SerializeField] private Camera cam;

    void LateUpdate()
    {
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 0f, 50f);
    }
}
