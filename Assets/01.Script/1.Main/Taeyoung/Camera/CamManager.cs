using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoSingleTon<CamManager>
{
    CinemachineTargetGroup cinemachineTargetGroup;

    public void Awake()
    {
        cinemachineTargetGroup = FindObjectOfType<CinemachineTargetGroup>();
    }

    public void AddTargetGroup(Transform target, float weight = 1f, float radius = 3f)
    {
        if (cinemachineTargetGroup.FindMember(target) > 0)
            return;

        cinemachineTargetGroup.AddMember(target, weight, radius);
    }

    public void RemoveTargetGroup(Transform target)
    {
        cinemachineTargetGroup.RemoveMember(target);
    }
}
