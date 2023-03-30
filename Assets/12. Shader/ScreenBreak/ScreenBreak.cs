using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBreak : MonoBehaviour
{
    private List<Transform> childTrm = new List<Transform>();
    private void Awake()
    {
        //Vector3 explosionPosition = new Vector3(45.833f, 0f, 0f);
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Transform>())
            {
                childTrm.Add(child.transform);
            }
        }

    }
    public void BreakScreen(Transform trm)
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.isKinematic = false;
                childRigidbody.AddExplosionForce(70f, trm.position, 15f);
               
            }
        }
    }
    
    public void InitPos()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Transform>())
            {
                child.position = childTrm[i].position;
                i++;
            }
        }
    }
}
