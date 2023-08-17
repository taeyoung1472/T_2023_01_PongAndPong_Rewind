using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabCollectionObjCutSceneTargetGroup : MonoBehaviour
{
    [SerializeField]
    private float _weight = 2f;

    public void TargetWeightSet()
    {
        GetComponent<CinemachineTargetGroup>().m_Targets[1].weight = _weight;
    }
}
