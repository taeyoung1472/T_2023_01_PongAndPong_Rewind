using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhonePosition : MonoBehaviour
{
    [SerializeField] private Transform phone;
    private Camera myCam;

    private void Start()
    {
        myCam = GetComponent<Camera>();
    }

    private void Update()
    {
        float frustumHeight = CalculateFrustumHeight(6);
        float phoneHeight = 6;

        float size = frustumHeight / phoneHeight;
        phone.transform.localScale = Vector3.one * size;

        //Debug.Log("Camera Frustum Height: " + frustumHeight);
    }

    private float CalculateFrustumHeight(float distanceFromCamera)
    {
        // 현재 활성화된 카메라의 수직 시야각과 카메라와의 거리를 사용하여 절두체 높이를 계산합니다.
        float frustumHeight = 2.0f * distanceFromCamera * Mathf.Tan(myCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        return frustumHeight;
    }
}
