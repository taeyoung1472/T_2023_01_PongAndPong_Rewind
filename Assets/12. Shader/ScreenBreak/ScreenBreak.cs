using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBreak : MonoBehaviour
{
    [SerializeField]private List<Vector3> childTrm = new List<Vector3>();
    private void Awake()
    {
        //Vector3 explosionPosition = new Vector3(45.833f, 0f, 0f);
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Transform>())
            {
                childTrm.Add(child.transform.position);
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
                //AddExplosionForce(���߷�, ������ġ, �ݰ�, ���� �ڱ��Ŀø��� ��)
                childRigidbody.AddExplosionForce(120f, trm.position, 7f);
               
            }
        }
    }
    
    public void InitPos()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.isKinematic = true;

            }
            if (child.GetComponent<Transform>())
            {
                child.position = childTrm[i];
                child.rotation = Quaternion.identity;
                i++;
            }
        }
    }
}
