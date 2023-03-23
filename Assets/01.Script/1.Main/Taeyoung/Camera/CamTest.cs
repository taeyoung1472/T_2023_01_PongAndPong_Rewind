using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTest : MonoBehaviour
{
    public Transform target;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            CamManager.Instance.AddTargetGroup(target);
        }
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            CamManager.Instance.RemoveTargetGroup(target);
        }
    }
}
