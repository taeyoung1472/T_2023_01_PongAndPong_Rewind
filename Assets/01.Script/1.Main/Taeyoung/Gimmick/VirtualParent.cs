using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualParent : MonoBehaviour
{
    [SerializeField] private Transform virtualParent;
    void Update()
    {
        transform.position = virtualParent.position;
    }
}
