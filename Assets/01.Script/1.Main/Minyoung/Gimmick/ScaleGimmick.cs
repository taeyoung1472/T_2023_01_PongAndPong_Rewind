using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ScaleGimmick : MonoBehaviour
{
    private Collider leftScaleCol;
    private Collider rightScaleCol;
    public float rayDistance = 5f;

    [SerializeField] private float totalLineLength;

    public int leftWeight = 0;
    public int rightWeight = 0;


    public bool isDowning;
    private float preLeftWeight;
    private float preRightWeight;
    private void Start()
    {
        leftScaleCol = transform.Find("Left").GetComponent<Collider>();
        rightScaleCol = transform.Find("Right").GetComponent<Collider>();
    }
    private void FixedUpdate()
    {
        LeftShootBoxcast();
        RightShootBoxcast();
    }
    private void Update()
    {
        CalculPos();
    }

    private void CalculPos()
    {
        float ratio;

        if(rightWeight == leftWeight)
        {
            Vector3 _pos = leftScaleCol.transform.position;
            leftScaleCol.transform.position = new Vector3(_pos.x, Mathf.Lerp(_pos.y, -totalLineLength / 1.5f, Time.deltaTime), _pos.z);

            _pos = rightScaleCol.transform.position;
            rightScaleCol.transform.position = new Vector3(_pos.x, Mathf.Lerp(_pos.y, -totalLineLength / 1.5f, Time.deltaTime), _pos.z);
            return;
        }

        if(leftWeight == 0)
        {
            ratio = 1;
        }
        else
            ratio = rightWeight / leftWeight;
        float rightLength = ratio * totalLineLength;

        if (rightWeight == 0)
        {
            ratio = 1;
        }
        else
            ratio = leftWeight / rightWeight;
        float leftLength = ratio * totalLineLength;

        Vector3 pos = leftScaleCol.transform.position;
        leftScaleCol.transform.position = new Vector3(pos.x, Mathf.Lerp(pos.y, -leftLength, Time.deltaTime), pos.z);

        pos = rightScaleCol.transform.position;
        rightScaleCol.transform.position = new Vector3(pos.x, Mathf.Lerp(pos.y, -rightLength, Time.deltaTime), pos.z);
    }

    void LeftShootBoxcast()
    {
        Vector3 boxCenter = leftScaleCol.bounds.center;
        Vector3 halfExtents = leftScaleCol.bounds.extents;

        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, halfExtents, transform.up, transform.rotation, rayDistance);

        leftWeight = 0;
        preLeftWeight = leftWeight;

        foreach (var h in hits)
        {
            if (h.transform == null)
                continue;

            ObjWeight obj = h.transform.GetComponent<ObjWeight>();
            if (obj == null)
                continue;

            leftWeight += obj.so.weight;
        }
    }
    void RightShootBoxcast()
    {
        Vector3 boxCenter = rightScaleCol.bounds.center;
        Vector3 halfExtents = rightScaleCol.bounds.extents;

        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, halfExtents, transform.up, transform.rotation, rayDistance);
        rightWeight = 0;
        preRightWeight = rightWeight;

        foreach (var h in hits)
        {
            if (h.transform == null)
                continue;

            ObjWeight obj = h.transform.GetComponent<ObjWeight>();
            if (obj == null)
                continue;

            rightWeight += obj.so.weight;
        }
    }
}
