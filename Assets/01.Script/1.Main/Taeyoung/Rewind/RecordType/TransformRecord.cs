using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TransformRecord : RecordObject
{
    [Header("[Check List]")]
    [SerializeField] private bool isRecordPosition;
    [SerializeField] private bool isRecordRotation;
    [SerializeField] private bool isRecordScale;

    [Header("[Data]")]
    private List<Vector3> positionList;
    private List<Quaternion> rotationList;
    private List<Vector3> scaleList;

    [Header("[DataLerp]")]
    private Vector3 curLerpPos;
    private Vector3 nextLerpPos;
    private Quaternion curLerpRot;
    private Quaternion nextLerpRot;
    private Vector3 curLerpScale;
    private Vector3 nextLerpScale;

    Rigidbody rb;

    private float aaa;

    RigidbodyConstraints rbConstraints;

    public void Start()
    {
        SaveRigidbodyData();
        Register();
    }

    private void Update()
    {
        aaa = RecordingPercent;
    }

    private void SaveRigidbodyData()
    {
        rb = GetComponent<Rigidbody>();

        rbConstraints = rb.constraints;
    }

    public override void OnRewindUpdate()
    {
        if (isRecordPosition)
        {
            transform.position = Vector3.Lerp(curLerpPos, nextLerpPos, RecordingPercent);
        }

        if (isRecordRotation)
        {
            transform.rotation = Quaternion.Lerp(curLerpRot, nextLerpRot, RecordingPercent);
        }

        if (isRecordScale)
        {
            transform.position = Vector3.Lerp(curLerpScale, nextLerpScale, RecordingPercent);
        }
    }

    public override void Register()
    {
        int totalCount = RewindManager.Instance.TotalRecordCount;

        if (isRecordPosition)
        {
            positionList = new()
            {
                Capacity = RewindManager.Instance.TotalRecordCount
            };
            positionList.AddRange(new Vector3[totalCount]);
            positionList[0] = transform.position;
        }

        if (isRecordRotation)
        {
            rotationList = new()
            {
                Capacity = RewindManager.Instance.TotalRecordCount
            };
            rotationList.AddRange(new Quaternion[totalCount]);
            rotationList[0] = transform.rotation;
        }

        if (isRecordScale)
        {
            scaleList = new()
            {
                Capacity = RewindManager.Instance.TotalRecordCount
            };
            scaleList.AddRange(new Vector3[totalCount]);
            scaleList[0] = transform.localScale;
        }

        RewindManager.Instance.RegistRecorder(this);
    }

    public override void DeRegister()
    {

    }

    public override void ApplyData(int index)
    {
        index = Mathf.Clamp(index, 0, rotationList.Count - 1);
        int nextIndex = Mathf.Clamp(index + 1, 0, rotationList.Count - 1);
        if (isRecordPosition)
        {
            curLerpPos = positionList[nextIndex];
            nextLerpPos = positionList[index];
        }

        if (isRecordRotation)
        {
            curLerpRot = rotationList[nextIndex];
            nextLerpRot = rotationList[index];
        }

        if (isRecordScale)
        {
            curLerpScale = scaleList[nextIndex];
            nextLerpScale = scaleList[index];
        }
    }

    public override void Recorde(int index)
    {
        if (isRecordPosition)
        {
            positionList[index] = transform.position;
        }

        if (isRecordRotation)
        {
            rotationList[index] = transform.rotation;
        }

        if (isRecordScale)
        {
            scaleList[index] = transform.localScale;
        }
    }

    public override void InitOnRewind()
    {
        curLerpPos = transform.position;
        nextLerpPos = transform.position;

        curLerpRot = transform.rotation;
        nextLerpRot = transform.rotation;

        curLerpScale = transform.localScale;
        nextLerpScale = transform.localScale;

        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public override void InitOnPlay()
    {
        rb.constraints = rbConstraints;
    }

    struct Vector3Bool
    {
        public bool x;
        public bool y;
        public bool z;
    }
}
