using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageCam : MonoBehaviour
{
    Camera cam;

    [SerializeField] private float edge;
    [SerializeField] private Transform top;
    [SerializeField] private Transform bottom;
    [SerializeField] private Transform right;
    [SerializeField] private Transform left;

    Vector3 mid;
    void Start()
    {
        cam = Camera.main;
        float frustumHeight = Mathf.Abs(top.position.y - bottom.position.y) + edge;
        float frustumWidth = frustumHeight * ((float)Screen.width / (float)Screen.height);

        Debug.Log($"X : {frustumWidth} Y : {frustumHeight}");

        mid = new Vector3((right.position.x + left.position.x) / 2, (top.position.y + bottom.position.y) / 2, cam.transform.position.z);

        cam.transform.position = mid;

        cam.fieldOfView = 2.0f * Mathf.Atan(frustumHeight * 0.5f / Mathf.Abs(cam.transform.position.z)) * Mathf.Rad2Deg;
    }
}
