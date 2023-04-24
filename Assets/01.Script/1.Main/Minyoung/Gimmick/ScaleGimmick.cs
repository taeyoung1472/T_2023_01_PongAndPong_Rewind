using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;

public class ScaleGimmick : GimmickObject
{
    public Collider leftCol;
    public Collider rightCol;
    [SerializeField] private float totalLength;

    public Vector3 leftOriginPos;
    public Vector3 rightOriginPos;

    public override void Init()
    {
        leftCol = transform.Find("Left").GetComponent<Collider>();
        rightCol = transform.Find("Right").GetComponent<Collider>();
    }

    public override void Awake()
    {
        base.Awake();
        Init();
    }
    private void Start()
    {
        leftOriginPos = leftCol.transform.position;
        rightOriginPos = rightCol.transform.position;
    }
    public void Update()
    {
        if (isRewind)
        {
            return;
        }
        Move();
    }
    public float left;
    public float right;

    private void Move()
    {
        float leftLength;
        float rightLength = 0;
         left = CalculLeftWeight();
        right = CalculRightWeight();

        if ((left == 0 && right == 0) || left == right)
        {
            rightLength = totalLength * 0.5f;
            leftLength = totalLength * 0.5f;
        }
        else if (left == 0)
        {
            rightLength = totalLength;
            leftLength = 0;
        }
        else if (right == 0)
        {
            rightLength = 0;
            leftLength = totalLength;
        }
        else
        {
            float total = right + left;
            float ratioRight = right * (1 / total);
            float ratioLeft = left * (1 / total);

            rightLength = totalLength * ratioRight;
            leftLength = totalLength * ratioLeft;
            Debug.Log(leftLength);
            Debug.Log(rightLength);
        }
        leftCol.transform.position = Vector3.Lerp(leftCol.transform.position, leftOriginPos + new Vector3(0, leftOriginPos.y - leftLength, 0), Time.deltaTime);
        rightCol.transform.position = Vector3.Lerp(rightCol.transform.position, rightOriginPos + new Vector3(0, rightOriginPos.y - rightLength, 0), Time.deltaTime);
    }

    private float CalculRightWeight()
    {
        Vector3 boxCenter = rightCol.bounds.center + Vector3.up * rightCol.bounds.size.y;
        Vector3 halfExtents = rightCol.bounds.size;
        Collider[] hitColliders = Physics.OverlapBox(boxCenter, halfExtents, Quaternion.identity);
        float rightWeight = 0;
        foreach (var col in hitColliders)
        {
            if (col.transform == null)
                continue;

            ObjWeight obj = col.transform.GetComponent<ObjWeight>();
            if (obj == null)
                continue;
            rightWeight += obj.so.weight;
        }

        return rightWeight;
    }

    private float CalculLeftWeight()
    {
        Vector3 boxCenter = leftCol.bounds.center + Vector3.up * rightCol.bounds.size.y;
        Vector3 halfExtents = leftCol.bounds.size;
        Collider[] hitColliders = Physics.OverlapBox(boxCenter, halfExtents, Quaternion.identity);
        float leftWeight = 0;
        foreach (var col in hitColliders)
        {
            if (col.transform == null)
                continue;

            ObjWeight obj = col.transform.GetComponent<ObjWeight>();
            if (obj == null)
                continue;
            leftWeight += obj.so.weight;
        }

        return leftWeight;
    }

    private void OnDrawGizmos()
    {
        //Vector3 boxCenter = rightCol.bounds.center + Vector3.up * rightCol.bounds.size.y;
        //Vector3 halfExtents = rightCol.bounds.size;
        //Gizmos.DrawWireCube(boxCenter, halfExtents);

        //boxCenter = leftCol.bounds.center + Vector3.up * rightCol.bounds.size.y;
        //halfExtents = leftCol.bounds.size;
        //Gizmos.DrawWireCube(boxCenter, halfExtents);
    }

}