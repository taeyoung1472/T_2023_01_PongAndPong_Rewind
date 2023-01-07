using System.Collections.Generic;
using UnityEngine;

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

    public void Start()
    {
        Register();
    }

    public void Update()
    {
        if (isRewinding || isEnd)
        {
            if (isRecordPosition)
            {
                transform.position = Vector3.Lerp(curLerpPos, nextLerpPos, recordingPercent);
            }

            if (isRecordRotation)
            {
                transform.rotation = Quaternion.Lerp(curLerpRot, nextLerpRot, recordingPercent);
            }

            if (isRecordScale)
            {
                transform.position = Vector3.Lerp(curLerpScale, nextLerpScale, recordingPercent);
            }
        }
    }

    public override void Register()
    {
        int totalCount = RewindManager.Instance.TotalRecordCount;

        if (isRecordPosition)
        {
            positionList = new();
            positionList.Capacity = RewindManager.Instance.TotalRecordCount;
            positionList.AddRange(new Vector3[totalCount]);
            positionList[0] = transform.position;
        }

        if (isRecordRotation)
        {
            rotationList = new();
            rotationList.Capacity = RewindManager.Instance.TotalRecordCount;
            rotationList.AddRange(new Quaternion[totalCount]);
            rotationList[0] = transform.rotation;
        }

        if (isRecordScale)
        {
            scaleList = new();
            scaleList.Capacity = RewindManager.Instance.TotalRecordCount;
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
        if (isRecordPosition)
        {
            curLerpPos = positionList[index + 1];
            nextLerpPos = positionList[index];
        }

        if (isRecordRotation)
        {
            curLerpRot = rotationList[index + 1];
            nextLerpRot = rotationList[index];
        }

        if (isRecordScale)
        {
            curLerpScale = scaleList[index + 1];
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
    }
}
