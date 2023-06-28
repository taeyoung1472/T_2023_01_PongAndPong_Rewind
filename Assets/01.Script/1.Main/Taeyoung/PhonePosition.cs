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
        // ���� Ȱ��ȭ�� ī�޶��� ���� �þ߰��� ī�޶���� �Ÿ��� ����Ͽ� ����ü ���̸� ����մϴ�.
        float frustumHeight = 2.0f * distanceFromCamera * Mathf.Tan(myCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        return frustumHeight;
    }
}
