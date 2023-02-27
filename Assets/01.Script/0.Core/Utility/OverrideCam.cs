using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverrideCam : MonoBehaviour
{
    [SerializeField] private Camera parentCam;
    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        cam.fieldOfView = parentCam.fieldOfView;
    }
}
