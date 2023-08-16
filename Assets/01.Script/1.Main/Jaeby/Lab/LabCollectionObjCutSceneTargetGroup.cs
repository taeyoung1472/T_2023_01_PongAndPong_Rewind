using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabCollectionObjCutSceneTargetGroup : MonoBehaviour
{
    public void TargetWeightSet()
    {
        GetComponent<CinemachineTargetGroup>().m_Targets[1].weight = 2.5f;
    }
}
