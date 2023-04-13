using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ScaleGimmick : GimmickObject
{
    [SerializeField] LayerMask scaleLayerMask;
    private Collider leftScaleCol;
    private Collider rightScaleCol;
    public float rayDistance = 5f;

    [SerializeField] private float totalLineLength;

    public int leftWeight = 0;
    public int rightWeight = 0;

    public bool isStart = false;
    public override void Init()
    {
        leftScaleCol = transform.Find("Left").GetComponent<Collider>();
        rightScaleCol = transform.Find("Right").GetComponent<Collider>();
    }

    public override void Awake()
    {
        base.Awake();
        Init();
    }
    public override void InitOnPlay()
    {
        base.InitOnPlay();
        isStart = true;
    }
    private void FixedUpdate()
    {
        if (isRewind || !isStart)
        {
            return;
        }
        LeftShootBoxcast();
        RightShootBoxcast();
    }
    private void Update()
    {
        if (isRewind || !isStart)
        {
            return;
        }
        CalculPos();
    }

    private void CalculPos()
    {
        float ratio;

        if(rightWeight == leftWeight)
        {
            Vector3 _pos = leftScaleCol.transform.localPosition;
            Debug.Log(_pos); 
           //leftScaleCol.transform.position = new Vector3(_pos.x, Mathf.Lerp(_pos.y, -totalLineLength / 1.5f, Time.deltaTime), _pos.z);
           leftScaleCol.transform.localPosition = new Vector3(_pos.x, Mathf.Lerp(_pos.y, 0, Time.deltaTime), _pos.z);

            _pos = rightScaleCol.transform.localPosition;
            //rightScaleCol.transform.position = new Vector3(_pos.x, Mathf.Lerp(_pos.y, -totalLineLength / 1.5f, Time.deltaTime), _pos.z);
            rightScaleCol.transform.localPosition = new Vector3(_pos.x, Mathf.Lerp(_pos.y, 0, Time.deltaTime), _pos.z);

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

        Vector3 pos = leftScaleCol.transform.localPosition;
        leftScaleCol.transform.localPosition = new Vector3(pos.x, Mathf.Lerp(pos.y, -leftLength, Time.deltaTime), pos.z);

        pos = rightScaleCol.transform.localPosition;
        rightScaleCol.transform.localPosition = new Vector3(pos.x, Mathf.Lerp(pos.y, -rightLength, Time.deltaTime), pos.z);
    }

    void LeftShootBoxcast()
    {
        Vector3 boxCenter = leftScaleCol.bounds.center;
        Vector3 halfExtents = leftScaleCol.bounds.extents;

        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, halfExtents, transform.up, transform.rotation, rayDistance, scaleLayerMask);

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
    }
    void RightShootBoxcast()
    {
        Vector3 boxCenter = rightScaleCol.bounds.center;
        Vector3 halfExtents = rightScaleCol.bounds.extents;

        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, halfExtents, transform.up, transform.rotation, rayDistance, scaleLayerMask);
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
    }

}
