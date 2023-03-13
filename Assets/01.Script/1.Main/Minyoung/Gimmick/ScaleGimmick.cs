using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleGimmick : MonoBehaviour
{
    private Collider leftScaleCol;
    private Collider rightScaleCol;
    public float rayDistance = 5f;

    public float leftWeight = 0f;
    public float rightWeight = 0f;
    private void Start()
    {
        leftScaleCol = transform.Find("Left").GetComponent<Collider>();
        rightScaleCol = transform.Find("Right").GetComponent<Collider>();
    }
    private void Update()
    {
        CheckDown();
    }
    private void FixedUpdate()
    {
        LeftShootBoxcast();
        RightShootBoxcast();

    }
    void LeftShootBoxcast()
    {
        Vector3 boxCenter = leftScaleCol.bounds.center;
        Vector3 halfExtents = leftScaleCol.bounds.extents;

        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, halfExtents, transform.up, transform.rotation, rayDistance);
        leftWeight = 0;

        foreach (var h in hits)
        {
            if (h.transform == null)
                continue;

            ObjWeight obj = h.transform.GetComponent<ObjWeight>();
            if (obj == null)
                continue;

            leftWeight += obj.so.weight;
        }
        Debug.Log(leftWeight);
    }
    void RightShootBoxcast()
    {
        Vector3 boxCenter = rightScaleCol.bounds.center;
        Vector3 halfExtents = rightScaleCol.bounds.extents;

        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, halfExtents, transform.up, transform.rotation, rayDistance);
        rightWeight = 0;

        foreach (var h in hits)
        {
            if (h.transform == null)
                continue;

            ObjWeight obj = h.transform.GetComponent<ObjWeight>();
            if (obj == null)
                continue;

            rightWeight += obj.so.weight;
        }
        Debug.Log(rightWeight);
    }

    public bool CheckDown()
    {
        if (leftWeight < rightWeight)
        {
            print("오른쪽이무거움");
            return true;
        }
        else
        {
            print("왼쪽이무거움");
            return false;
        }
    }

    public void WeightDownPos()
    {
        CheckDown();
    }
}
