using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewindManager : MonoSingleTon<RewindManager>
{
    // 플레이 시간
    [SerializeField] private int playTime;

    // 기록 감도
    [SerializeField] private readonly float recordeTurm = 0.1f;
    public float RecordeTurm { get { return recordeTurm; } }

    // 시간 체크용
    private float timer = 0.0f;

    // Index
    private int curRecordingIndex;
    public int CurRecordingIndex
    {
        get { return curRecordingIndex; }
        set
        {
            curRecordingIndex = value;
            OnTimeChanging?.Invoke(value);
        }
    }

    private int totalRecordCount;
    public int TotalRecordCount { get { return totalRecordCount; } }
    public float CurRecordingPercent { get { return Mathf.Clamp(1 - (timer - Time.time) / recordeTurm, 0, 1); } }

    // Logic
    private bool isRewinding = false;
    public bool IsRewinding { get { return isRewinding; } set { isRewinding = value; } }
    private bool isEnd = false;
    public bool IsEnd { get { return isEnd; } set { isEnd = value; } }
    private bool checkEnd = false;

    // Event
    public Action<int> OnTimeChanging;

    // RecordObjectList
    private List<RecordObject> recordObjectList = new();
    private List<RecordObject> rewindObjectList = new();

    // totalRecord 계산
    public void OnValidate()
    {
        totalRecordCount = Mathf.RoundToInt(playTime / recordeTurm);
    }

    public void Start()
    {
        timer = Time.time + recordeTurm;
        totalRecordCount = Mathf.RoundToInt(playTime / recordeTurm);
    }

    public void Update()
    {
        if (!IsEnd)
        {
            Recorde();
        }
        else
        {
            // 엔딩
        }
        ExcuteUpdate();
    }

    private void ExcuteUpdate()
    {
        if (IsEnd)
            return;

        ExcuteUpdate(RecordType.Default, IsRewinding);
        ExcuteUpdate(RecordType.Rewind, !IsRewinding);
    }

    private void Recorde()
    {
        if (Time.time > timer)
        {
            if (checkEnd)
                IsEnd = true;

            // 타이머 초기화
            timer = Time.time + recordeTurm;

            // 역행중
            if (IsRewinding)
            {
                CurRecordingIndex--;

                ApplyData(RecordType.Default, CurRecordingIndex);
                RecordData(RecordType.Rewind, (totalRecordCount - 1) - CurRecordingIndex);

                // 게임 끝남
                if (CurRecordingIndex == 0)
                {
                    checkEnd = true;
                }
            }

            // 순행중
            else
            {
                CurRecordingIndex++;

                RecordData(RecordType.Default, CurRecordingIndex);
                ApplyData(RecordType.Rewind, (totalRecordCount - 1) - CurRecordingIndex);

                // 순행 끝남
                if (curRecordingIndex == totalRecordCount - 1)
                {
                    // 역행 시작
                    IsRewinding = true;

                    // 초기화
                    for (int i = 0; i < recordObjectList.Count; i++)
                    {
                        recordObjectList[i].InitOnRewind();
                    }

                    Debug.Log("역행 시작!");
                }
            }
        }
    }

    public void RecordData(RecordType type, int index)
    {
        List<RecordObject> targetList = GetRecordObjectByType(type);

        for (int i = 0; i < targetList.Count; ++i)
        {
            targetList[i].Recorde(index);
        }
    }

    public void ApplyData(RecordType type, int index)
    {
        List<RecordObject> targetList = GetRecordObjectByType(type);

        for (int i = 0; i < targetList.Count; ++i)
        {
            targetList[i].ApplyData(index);
        }
    }

    public void ExcuteUpdate(RecordType type, bool isRewinding)
    {
        List<RecordObject> targetList = GetRecordObjectByType(type);

        for (int i = 0; i < targetList.Count; ++i)
        {
            if (!isRewinding)
                targetList[i].OnUpdate();
            else if (isRewinding)
                targetList[i].OnRewindUpdate();
        }
    }

    private List<RecordObject> GetRecordObjectByType(RecordType type)

    {
        List<RecordObject> returnList;

        switch (type)
        {
            case RecordType.Default:
                returnList = recordObjectList;
                break;
            case RecordType.Rewind:
                returnList = rewindObjectList;
                break;
            default:
                Debug.LogWarning("Type 이 이상해");
                returnList = null;
                break;
        }

        return returnList;
    }

    public void RegistRecorder(RecordObject recordObject)
    {
        // 역행중
        if (IsRewinding)
        {
            rewindObjectList.Add(recordObject);
        }

        else
        {
            recordObjectList.Add(recordObject);
        }
    }
}

public enum RecordType
{
    Default,
    Rewind,
    End,
}