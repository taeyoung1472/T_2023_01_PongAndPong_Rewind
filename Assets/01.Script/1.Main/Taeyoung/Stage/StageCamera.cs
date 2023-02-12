using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class StageCamera : MonoSingleTon<StageCamera>
{
    [SerializeField] private CinemachineVirtualCamera cam;
    public void SetCameraFov(Transform rightTop, Transform leftBottom, float edge = 3)
    {
        //StageCam
        float right, left, top, bottom;
        right = rightTop.position.x;
        left = leftBottom.position.x;
        top = rightTop.position.y;
        bottom = leftBottom.position.y;

        float x, y;
        x = right - left;
        y = top - bottom;

        float frustumHeight = 0, frustumWidth = 0;
        if (y > x)
        {
            frustumHeight = y + edge;
        }
        else
        {
            frustumWidth = x + edge;
            frustumHeight = frustumWidth / ((float)Screen.width / (float)Screen.height);
        }

        cam.transform.position = new Vector3((right + left) / 2, (top + bottom) / 2, cam.transform.position.z);

        //Debug.Log(2.0f * Mathf.Atan(frustumHeight * 0.5f / Mathf.Abs(cam.transform.position.z)) * Mathf.Rad2Deg);

        cam.m_Lens.FieldOfView = 2.0f * Mathf.Atan(frustumHeight * 0.5f / Mathf.Abs(cam.transform.position.z)) * Mathf.Rad2Deg;
    }
}
