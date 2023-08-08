using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabCCTV : MonoBehaviour
{
    [SerializeField] private float rotationTime;
    [SerializeField] private float rotationIntencity;
    private Vector3 initRot;

    private void Awake()
    {
        initRot = transform.eulerAngles;
    }

    private void Update()
    {
        Vector3 angle = initRot;
        angle.y = initRot.y + Mathf.Sin(Time.time / rotationTime) * rotationIntencity;
        transform.eulerAngles = angle;
    }
}
