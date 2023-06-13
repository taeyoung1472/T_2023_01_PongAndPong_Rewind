using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFocus : MonoBehaviour
{
    [SerializeField] private Transform focusObject;
    private void OnTriggerEnter(Collider other)
    {
        CamManager.Instance.AddTargetGroup(focusObject);
    }

    private void OnTriggerExit(Collider other)
    {
        CamManager.Instance.RemoveTargetGroup(focusObject);
    }
}
