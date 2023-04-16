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
    public override void Init()
    {

    }

    public void Update()
    {
        Move();
    }

    private void Move()
    {
        float leftLength = 0;
        float rightLength = 0;
        float left = CalculLeftWeight();
        float right = CalculRightWeight();

        if((left == 0 && right == 0) || left == right)
        {
            rightLength = totalLength * 0.5f;
            leftLength = totalLength * 0.5f;
        }
        else if(left == 0)
        {
            rightLength = totalLength;
            leftLength = 0;
        }
        else if(right == 0)
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
        }

        leftCol.transform.position = Vector3.Lerp(leftCol.transform.position, new Vector3(transform.position.x - 5, transform.position.y - leftLength, transform.position.z), Time.deltaTime);
        rightCol.transform.position = Vector3.Lerp(rightCol.transform.position, new Vector3(transform.position.x + 5, transform.position.y - rightLength, transform.position.z), Time.deltaTime);
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
        Vector3 boxCenter = rightCol.bounds.center + Vector3.up * rightCol.bounds.size.y;
        Vector3 halfExtents = rightCol.bounds.size;
        Gizmos.DrawWireCube(boxCenter, halfExtents);

        boxCenter = leftCol.bounds.center + Vector3.up * rightCol.bounds.size.y;
        halfExtents = leftCol.bounds.size;
        Gizmos.DrawWireCube(boxCenter, halfExtents);
    }

    #region MyRegion
    //public Collider leftCol;
    //public Collider rightCol;
    //public int leftWeight;
    //public int rightWeight;
    //public float ratio;
    //public float totalLineLength;

    //public override void Awake()
    //{
    //    base.Awake();
    //    leftCol = transform.Find("Left").GetComponent<Collider>();
    //    rightCol = transform.Find("Right").GetComponent<Collider>();
    //}
    //private void Start()
    //{
    //    MoveScale();
    //}
    //private void Update()
    //{
    //}

    //public override void Init()
    //{
    //}
    //private void FixedUpdate()
    //{
    //   // LeftWeightCheck();
    //   // RightWeightCheck();
    //}
    //public void MoveScale()
    //{
    //    if (rightWeight == leftWeight)
    //    {
    //        leftCol.transform.DOLocalMoveY(0f, 2f);
    //        rightCol.transform.DOLocalMoveY(0f, 2f);
    //        return;
    //    }
    //    else
    //    {
    //        if (leftWeight == 0)
    //        {

    //            ratio = 1;
    //        }
    //        else
    //            ratio = leftWeight / rightWeight;
    //        float leftLineLength = totalLineLength * ratio; //3.3

    //        if (rightWeight == 0)
    //        {
    //            ratio = 1; 

    //        }
    //        else
    //            ratio = rightWeight / leftWeight;
    //        float rightLength = ratio * totalLineLength; // 30

    //        leftCol.transform.DOLocalMoveY(-leftLineLength, leftLineLength /3);
    //        rightCol.transform.DOLocalMoveY(rightLength, rightLength / 3);
    //    }
    //}
    //public void LeftWeightCheck()
    //{
    //    Vector3 boxCenter = leftCol.bounds.center;
    //    Vector3 halfExtents = leftCol.bounds.extents;
    //    Collider[] hitColliders = Physics.OverlapBox(boxCenter, halfExtents, Quaternion.identity);
    //    leftWeight = 0;
    //    foreach (var col in hitColliders)
    //    {
    //        if (col.transform == null)
    //            continue;

    //        ObjWeight obj = col.transform.GetComponent<ObjWeight>();
    //        if (obj == null)
    //            continue;
    //        leftWeight += obj.so.weight;
    //    }
    //}

    //public List<Collider> rightColList = new List<Collider>();
    //public void RightWeightCheck()
    //{
    //    Vector3 boxCenter = rightCol.bounds.center;
    //    Vector3 halfExtents = rightCol.bounds.extents;
    //    Collider[] hitColliders = Physics.OverlapBox(boxCenter, halfExtents, Quaternion.identity);
    //    foreach (var col in hitColliders)
    //    {
    //        if (col.transform == null)
    //            continue;
    //        ObjWeight obj = col.transform.GetComponent<ObjWeight>();
    //        if (obj == null)
    //            continue;
    //        rightWeight += obj.so.weight;
    //    }
    //    if (IsEqual(hitColliders, rightColList) == false) //서로 다르면
    //    {

    //    }


    //}
    //public bool IsEqual(Collider[] col1, List<Collider> col2)
    //{
    //    if (col1.Count() != col2.Count())
    //    {
    //        return false;
    //    }
    //    foreach (var item in col1)
    //    {
    //        if (!col2.Contains(item))
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}
    //public void OnDrawGizmos()
    //{
    //    //Gizmos.color = Color.red;
    //    //Gizmos.DrawWireCube(transform.Find("Left").GetComponent<BoxCollider>().center,
    //    //   transform.Find("Left").GetComponent<BoxCollider>().size);
    //}
    #endregion
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
