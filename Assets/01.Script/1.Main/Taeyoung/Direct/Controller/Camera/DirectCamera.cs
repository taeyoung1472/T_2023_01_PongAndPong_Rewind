using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DirectCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    Transform camFollowTrans;
    private float edge = 5f;

    List<Transform> focusList = new();

    private void Start()
    {
        camFollowTrans = new GameObject().GetComponent<Transform>();
        camFollowTrans.SetParent(transform);
        camFollowTrans.name = "VcamFollow";

        vcam.Follow = camFollowTrans;
    }

    private void Update()
    {
        SetFollowTransPosition();
        SetFov();
    }

    private void SetFollowTransPosition()
    {
        if (focusList.Count == 0)
            return;

        Vector3 pos = Vector3.zero;
        for (int i = 0; i < focusList.Count; i++)
        {
            pos += focusList[i].position;
        }
        camFollowTrans.position = pos / focusList.Count;
    }

    private void SetFov()
    {
        if (focusList.Count == 0)
            return;

        float top, bottom, right, left;

        top = bottom = focusList[0].position.y;
        right = left = focusList[0].position.x;

        for (int i = 0; i < focusList.Count; i++)
        {
            if (focusList[i].position.y > top)
                top = focusList[i].position.y;
            if (focusList[i].position.y < bottom)
                bottom = focusList[i].position.y;
            if (focusList[i].position.x < left)
                left = focusList[i].position.x;
            if (focusList[i].position.x > right)
                right = focusList[i].position.x;
        }

        float x, y;
        x = right - left;
        y = top - bottom;

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

        vcam.m_Lens.FieldOfView = Mathf.Lerp(vcam.m_Lens.FieldOfView, 2.0f * Mathf.Atan(frustumHeight * 0.5f / Mathf.Abs(vcam.transform.position.z)) * Mathf.Rad2Deg, Time.deltaTime);
    }

    public void AddFocusObject(Transform trans)
    {
        focusList.Add(trans);
    }

    public void RemoveFocusObject(Transform trans)
    {
        focusList.Remove(trans);
    }

    public void SetCameraEdge(float value)
    {
        edge = value;
    }
}
