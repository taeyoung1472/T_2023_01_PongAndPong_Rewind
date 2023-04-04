using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusObject : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log($"Enabe {name}");
        CamManager.Instance.AddTargetGroup(transform);
    }

    private void OnDisable()
    {
        Debug.Log($"Disable {name}");
        if (CamManager.Instance == null)
            return;

        CamManager.Instance.RemoveTargetGroup(transform);
    }
}
