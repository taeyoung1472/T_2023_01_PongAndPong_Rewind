using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ScaleGimmick : GimmickObject
{
    public Collider leftCol;
    public Collider rightCol;
    public float leftWeight;
    public void Awake()
    {
        leftCol = transform.Find("left").GetComponent<Collider>();
    }
    private void Update()
    {

    }

    public override void Init()
    {
    }
    private void FixedUpdate()
    {
        LeftWeightCheck();
    }

    public void LeftWeightCheck()
    {
        Vector3 boxCenter = leftCol.bounds.center;
        Vector3 halfExtents = leftCol.bounds.extents;
        Collider[] hitColliders = Physics.OverlapBox(boxCenter, halfExtents, Quaternion.identity);

        foreach (var col in hitColliders)
        {
            if (col.transform == null)
                continue;

            ObjWeight obj = col.transform.GetComponent<ObjWeight>();
            if (obj == null)
                continue;

            leftWeight += obj.so.weight;
        }
    }
    public void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(transform.Find("Left").GetComponent<BoxCollider>().center,
        //   transform.Find("Left").GetComponent<BoxCollider>().size);
    }
}

        //[SerializeField] LayerMask scaleLayerMask;
        //private Collider leftScaleCol;
        //private Collider rightScaleCol;
        //private Rigidbody leftRb;
        //private Rigidbody rightRb;
        //public float rayDistance = 5f;

        //[SerializeField] private float totalLineLength;

        //public int leftWeight = 0;
        //public int rightWeight = 0;

        //public bool isStart = false;
        //public override void Init()
        //{
        //    leftScaleCol = transform.Find("Left").GetComponent<Collider>();
        //    rightScaleCol = transform.Find("Right").GetComponent<Collider>();
        //    leftRb = transform.Find("Left").GetComponent<Rigidbody>();
        //    rightRb = transform.Find("Right").GetComponent<Rigidbody>();
        //}

        //public override void Awake()
        //{
        //    base.Awake();
        //    Init();
        //}
        //public override void InitOnPlay()
        //{
        //    base.InitOnPlay();
        //    isStart = true;
        //}
        //private void FixedUpdate()
        //{
        //    if (isRewind || !isStart)
        //    {
        //        return;
        //    }
        //    LeftShootBoxcast();
        //    RightShootBoxcast();
        //    CalculPos();
        //}
        //private void Update()
        //{
        //}

        //private void CalculPos()
        //{
        //    float ratio;

        //    if(rightWeight == leftWeight)
        //    {
        //        Vector3 _pos = leftRb.transform.localPosition;
        //        Debug.Log(_pos);
        //        leftRb.MovePosition(new Vector3(_pos.x, 0, _pos.z) * Time.deltaTime);
        //       //leftScaleCol.transform.position = new Vector3(_pos.x, Mathf.Lerp(_pos.y, -totalLineLength / 1.5f, Time.deltaTime), _pos.z);
        //       //leftScaleCol.transform.localPosition = new Vector3(_pos.x, Mathf.Lerp(_pos.y, 0, Time.deltaTime), _pos.z);

        //        _pos = rightRb.transform.localPosition;
        //        rightRb.MovePosition(new Vector3(_pos.x, 0, _pos.z) * Time.deltaTime);
        //        //rightScaleCol.transform.position = new Vector3(_pos.x, Mathf.Lerp(_pos.y, -totalLineLength / 1.5f, Time.deltaTime), _pos.z);
        //        //rightScaleCol.transform.localPosition = new Vector3(_pos.x, Mathf.Lerp(_pos.y, 0, Time.deltaTime), _pos.z);

        //        return;
        //    }

        //    if(leftWeight == 0)
        //    {
        //        ratio = 1;
        //    }
        //    else
        //        ratio = rightWeight / leftWeight;
        //    float rightLength = ratio * totalLineLength;

        //    if (rightWeight == 0)
        //    {
        //        ratio = 1;
        //    }
        //    else
        //        ratio = leftWeight / rightWeight;
        //    float leftLength = ratio * totalLineLength;

        //    Vector3 pos = leftRb.transform.position;
        //    //  Vector3 pos = leftScaleCol.transform.position;
        //    leftRb.MovePosition(new Vector3(pos.x, -leftLength, pos.z) * Time.deltaTime); //position = new Vector3(pos.x, Mathf.Lerp(pos.y, -leftLength, Time.deltaTime), pos.z);

        //    pos = rightRb.transform.position;
        //    rightRb.MovePosition(new Vector3(pos.x, -rightLength, pos.z) * Time.deltaTime); //position = new Vector3(pos.x, Mathf.Lerp(pos.y, -leftLength, Time.deltaTime), pos.z);

        //}

        //void LeftShootBoxcast()
        //{
        //    Vector3 boxCenter = leftScaleCol.bounds.center;
        //    Vector3 halfExtents = leftScaleCol.bounds.extents;

        //    RaycastHit[] hits = Physics.BoxCastAll(boxCenter, halfExtents, transform.up, transform.rotation, rayDistance, scaleLayerMask);
        //    leftWeight = 0;

        //    foreach (var h in hits)
        //    {
        //        if (h.transform == null)
        //            continue;

        //        ObjWeight obj = h.transform.GetComponent<ObjWeight>();
        //        if (obj == null)
        //            continue;

        //        leftWeight += obj.so.weight;
        //        Debug.Log(leftWeight);
        //    }
        //}
        //void RightShootBoxcast()
        //{
        //    Vector3 boxCenter = rightScaleCol.bounds.center;
        //    Vector3 halfExtents = rightScaleCol.bounds.extents;

        //    RaycastHit[] hits = Physics.BoxCastAll(boxCenter, halfExtents, transform.up, transform.rotation, rayDistance, scaleLayerMask);
        //    rightWeight = 0;

        //    foreach (var h in hits)
        //    {
        //        if (h.transform == null)
        //            continue;

        //        ObjWeight obj = h.transform.GetComponent<ObjWeight>();
        //        if (obj == null)
        //            continue;

        //        rightWeight += obj.so.weight;
        //    }
        //}
