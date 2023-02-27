using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DirectCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    private CinemachineBasicMultiChannelPerlin noise;

    Transform camFollowTrans;
    private float edge = 5f;

    List<Transform> focusList = new();  
    List<AplyingCameraDirectData> cameraDirectList = new();

    float shakeAmplitude = 0.0f;
    float shakeFrequency = 0.0f;
    float zoomValue = 1.0f;
    Vector3 prevRotateValue = Vector3.zero;
    Vector3 rotateValue = Vector3.zero;
    Vector3 positionValue = Vector3.zero;

    private void Start()
    {
        camFollowTrans = new GameObject().GetComponent<Transform>();
        camFollowTrans.SetParent(transform);
        camFollowTrans.name = "VcamFollow";

        vcam.Follow = camFollowTrans;
        noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        CalculCameraDirect();
        ApplyCameraDirect();
    }

    private void ApplyCameraDirect()
    {
        SetFov();
        SetShake();
        SetPosition();
        SetRotation();
    }

    private void SetRotation()
    {
        prevRotateValue = Vector3.Lerp(prevRotateValue, rotateValue, Time.deltaTime);
        vcam.transform.eulerAngles = prevRotateValue;
    }

    private void SetPosition()
    {
        if (focusList.Count == 0)
            return;

        Vector3 pos = Vector3.zero;
        for (int i = 0; i < focusList.Count; i++)
        {
            pos += focusList[i].position;
        }

        camFollowTrans.position = (pos / focusList.Count) + positionValue;
    }

    private void SetShake()
    {
        noise.m_AmplitudeGain = Mathf.Lerp(noise.m_AmplitudeGain, shakeAmplitude, Time.deltaTime);
        noise.m_FrequencyGain = Mathf.Lerp(noise.m_FrequencyGain, shakeFrequency, Time.deltaTime);
    }

    private void CalculCameraDirect()
    {
        shakeAmplitude = 0.0f;
        shakeFrequency = 0.0f;
        zoomValue = 1.0f;
        rotateValue = Vector3.zero;
        positionValue = Vector3.zero;

        for (int i = 0; i < cameraDirectList.Count; i++)
        {
            AplyingCameraDirectData direct = cameraDirectList[i];

            float percent = direct.timer / direct.data.directTime;
            cameraDirectList[i].timer += Time.deltaTime;

            for (int j = 0; j < direct.data.typeList.Count; j++)
            {
                switch (direct.data.typeList[j])
                {
                    case CameraDirectType.Shake:
                        shakeAmplitude += direct.data.shakeAmplitude * direct.data.shakeCurve.Evaluate(percent);
                        shakeFrequency += direct.data.shakeFrequency * direct.data.shakeCurve.Evaluate(percent);
                        break;
                    case CameraDirectType.Zoom:
                        zoomValue = zoomValue * direct.data.zoomValue * direct.data.zoomCurve.Evaluate(percent);
                        break;
                    case CameraDirectType.Rotate:
                        rotateValue += direct.data.rotateValue * direct.data.rotateCurve.Evaluate(percent);
                        break;
                    case CameraDirectType.Position:
                        positionValue += direct.data.positionValue * direct.data.positionCurve.Evaluate(percent);
                        break;
                }
            }
        }

        for (int i = 0; i < cameraDirectList.Count; i++)
        {
            if(cameraDirectList[i].timer > cameraDirectList[i].data.directTime)
            {
                cameraDirectList.Remove(cameraDirectList[i]);
                i--;
            }
        }
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

        vcam.m_Lens.FieldOfView = Mathf.Lerp(vcam.m_Lens.FieldOfView, 2.0f * Mathf.Atan(frustumHeight * 0.5f / Mathf.Abs(vcam.transform.position.z)) * Mathf.Rad2Deg / zoomValue, Time.deltaTime);
    }

    public void AddFocusObject(Transform trans)
    {
        focusList.Add(trans);
    }

    public void RemoveFocusObject(Transform trans)
    {
        focusList.Remove(trans);
    }

    public void AddCameraDirect(CameraDirectData data)
    {
        cameraDirectList.Add(new AplyingCameraDirectData(data));
    }

    public void SetCameraEdge(float value)
    {
        edge = value;
    }

    class AplyingCameraDirectData
    {
        public AplyingCameraDirectData(CameraDirectData _data)
        {
            data = _data;
            timer = 0;
        }
        public float timer;
        public CameraDirectData data;
    }
}
