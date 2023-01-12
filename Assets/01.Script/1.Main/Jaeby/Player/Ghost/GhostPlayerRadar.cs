using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlayerRadar : MonoBehaviour
{
    [SerializeField]
    private LayerMask _layerMask;

    private void OnTriggerEnter(Collider other)
    {
        if ((_layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            ViewGimmickInfo(other);
        }
    }

    private void ViewGimmickInfo(Collider collider)
    {

    }
}
