using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDragCamMove : MonoBehaviour
{
    private Vector3 Origin;
    private Vector3 Difference;
    private Vector3 ResetCamera;

    private bool drag = false;

    private Camera cam;


    private void Awake()
    {
        cam = Camera.main;
    }
    private void Start()
    {
        ResetCamera = cam.transform.position;
    }


    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Difference = (cam.ScreenToViewportPoint(Input.mousePosition)) - cam.transform.position;
            Difference.z = 0;
            if(drag == false)
            {
                drag = true;
                Origin = cam.ScreenToViewportPoint(Input.mousePosition);
                Origin.z = 0;
            }

        }
        else
        {
            drag = false;
        }

        if (drag)
        {
            transform.position = Origin - Difference;
        }

        if (Input.GetMouseButton(1))
            transform.position = ResetCamera;

    }
}
