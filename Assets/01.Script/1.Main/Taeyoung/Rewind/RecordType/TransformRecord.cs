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

    RigidbodyConstraints rbConstraints;

    public void Awake()
    {
        SaveRigidbodyData();
        Register();
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
            transform.localScale = Vector3.Lerp(curLerpScale, nextLerpScale, RecordingPercent);
        }
    }

    public override void Register()
    {
        int totalCount = RewindManager.Instance.TotalRecordCount;

        if (isRecordPosition)
        {
            positionList = new(RewindManager.Instance.TotalRecordCount);
            positionList.AddRange(new Vector3[totalCount]);
            positionList[InitIndex] = transform.position;
        }

        if (isRecordRotation)
        {
            rotationList = new(RewindManager.Instance.TotalRecordCount);
            rotationList.AddRange(new Quaternion[totalCount]);
            rotationList[InitIndex] = transform.rotation;
        }

        if (isRecordScale)
        {
            scaleList = new(RewindManager.Instance.TotalRecordCount);
            scaleList.AddRange(new Vector3[totalCount]);
            scaleList[InitIndex] = transform.localScale;
        }

        RewindManager.Instance.RegistRecorder(this);
    }

    public override void DeRegister()
    {

    }

    public override void ApplyData(int index, int nextIndexDiff)
    {
        index = Mathf.Clamp(index, 0, TotalRecordCount - 1);
        int nextIndex = Mathf.Clamp(index + nextIndexDiff, 0, TotalRecordCount - 1);
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

}
