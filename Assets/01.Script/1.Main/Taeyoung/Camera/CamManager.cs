using Cinemachine;
using UnityEngine;

public class CamManager : MonoSingleTon<CamManager>
{
    CinemachineTargetGroup cinemachineTargetGroup;
    CinemachineTargetGroup CinemachineTargetGroup
    {
        get
        {
            if (cinemachineTargetGroup == null)
            {
                cinemachineTargetGroup = FindObjectOfType<CinemachineTargetGroup>();
            }
            return cinemachineTargetGroup;
        }
    }

    public void AddTargetGroup(Transform target, float weight = 1f, float radius = 3f)
    {
        if (CinemachineTargetGroup.FindMember(target) > 0)
            return;

        CinemachineTargetGroup.AddMember(target, weight, radius);
    }

    public void RemoveTargetGroup(Transform target)
    {
        if (CinemachineTargetGroup == null)
            return;

        CinemachineTargetGroup.RemoveMember(target);
    }
}
