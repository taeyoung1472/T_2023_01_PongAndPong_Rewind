using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusObject : MonoBehaviour
{
    private void OnEnable()
    {
        CamManager.Instance.AddTargetGroup(transform);
    }

    private void OnDisable()
    {
        if (CamManager.Instance == null)
            return;

        CamManager.Instance.RemoveTargetGroup(transform);
    }
}
