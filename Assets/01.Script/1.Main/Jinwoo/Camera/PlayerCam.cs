using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCam : MonoBehaviour
{
    public int a;
    private static CinemachineVirtualCamera vCam;

    private void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
    }

    public static void SetTarget(Transform target)
    {
        vCam.Follow = target;

    }

}
