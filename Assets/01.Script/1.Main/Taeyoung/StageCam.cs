using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
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

        float x, y;
        x = right.position.x - left.position.x;
        y = top.position.y - bottom.position.y;

        float frustumHeight, frustumWidth;
        if (y > x)
        {
            frustumHeight = y + edge;
        }
        else
        {
            frustumWidth = x + edge;
            frustumHeight = frustumWidth * ((float)Screen.height / (float)Screen.width);
        }

        mid = new Vector3((right.position.x + left.position.x) / 2, (top.position.y + bottom.position.y) / 2, cam.transform.position.z);

        cam.transform.position = mid;

        cam.fieldOfView = 2.0f * Mathf.Atan(frustumHeight * 0.5f / Mathf.Abs(cam.transform.position.z)) * Mathf.Rad2Deg;
    }
}
