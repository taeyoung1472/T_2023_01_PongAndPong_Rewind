using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabCCTV : MonoBehaviour
{
    [SerializeField] private float rotationTime;
    [SerializeField] private float rotationIntencity;
    private Vector3 initRot;
    Camera cam;
    float timer = 0.0f;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        initRot = transform.eulerAngles;
    }

    private void Update()
    {
        Vector3 angle = initRot;
        angle.y = initRot.y + Mathf.Sin(Time.time / rotationTime) * rotationIntencity;
        transform.eulerAngles = angle;

        if (cam.enabled)
        {
            cam.enabled = false;
        }

        timer += Time.deltaTime;
        if(timer > 0)
        {
            timer = -Random.Range(0.15f, 0.3f);
            cam.enabled = true;
        }
    }
}
