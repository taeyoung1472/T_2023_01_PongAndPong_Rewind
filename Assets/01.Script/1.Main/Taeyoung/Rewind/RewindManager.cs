using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RewindManager : MonoSingleTon<RewindManager>
{
    // 플레이 시간
    [SerializeField] private int playTime;

    // 기록 감도
    private readonly float recordeTurm = 0.1f;

    public float RecordeTurm { get { return recordeTurm; } }

    // 시간 체크용
    public float timer = 0.0f;

    // Volume
    [Header("[Volume]")]
    [SerializeField] private GameObject defaultVolume;
    [SerializeField] private GameObject rewindVolume;

    // Index
    private int curRecordingIndex;
    public int CurRecordingIndex
    {
        get { return curRecordingIndex; }
        set
        {
            lastChangeTime = Time.time;

            value = Mathf.Clamp(value, 0, totalRecordCount - 1);
            
            int diff = curRecordingIndex - value;

            if (diff < 0)
            {
                defaultVolume.SetActive(true);
                rewindVolume.SetActive(false); 
            }
            else
            {
                defaultVolume.SetActive(false);
                rewindVolume.SetActive(true);
            }

            curRecordingIndex = value;
            OnTimeChanging?.Invoke(value);

            int defaultIndex = value;
            int rewindIndex = (totalRecordCount - 1) - value;

            // 분석
            if (IsEnd)
            {
                ApplyData(RecordType.Default, defaultIndex, diff);
                ApplyData(RecordType.Rewind, rewindIndex, -diff);
            }

            // 순행
            else if (!IsRewinding)
            {
                RecordData(RecordType.Default, defaultIndex);
                ApplyData(RecordType.Rewind, rewindIndex, diff);
            }

            // 역행
            else
            {
                RecordData(RecordType.Rewind, rewindIndex);
                ApplyData(RecordType.Default, defaultIndex, diff);
            }
        }
    }

    private int totalRecordCount;
    public int TotalRecordCount { get { return totalRecordCount; } }
    private float lastChangeTime;
    public float CurRecordingPercent { get { return Mathf.Clamp((Time.time - lastChangeTime) / recordeTurm, 0, 1); } }

    // Logic
    private bool isRewinding = false;
    public bool IsRewinding 
    { 
        get { return isRewinding; }
        set 
        {
            isRewinding = value;
            RecordObjectInit(RecordType.Default);
            RecordObjectInit(RecordType.Rewind);
        }
    }
    private bool isEnd = false;
    public bool IsEnd { get { return isEnd; } set { isEnd = value; EndManager.Instance.ActivePanel(); } }

    // Event
    public Action<int> OnTimeChanging;

    // RecordObjectList
    private List<RecordObject> recordObjectList = new();
    private List<RecordObject> rewindObjectList = new();

    // Logic
    [SerializeField] private bool isReadyToStart;

    public void Awake()
    {
        totalRecordCount = Mathf.RoundToInt(playTime / recordeTurm);
    }

    private IEnumerator Start()
    {
        timer = Time.time + recordeTurm;

        Time.timeScale = 0;
        yield return new WaitUntil(() => isReadyToStart);
        Time.timeScale = 1;

        IsRewinding = false;
    }

    public void Update()
    {
        ExcuteUpdate();
        LifeCycle();

        if (Input.GetKeyDown(KeyCode.P))
        {
            isReadyToStart = true;
        }
    }

    public void Play()
    {
        IsRewinding = false;
    }

    public void ReWind()
    {
        IsRewinding = true;
    }

    private void LifeCycle()
    {
        if (IsEnd) return;
        if (Time.time > timer)
        {
            timer = Time.time + recordeTurm;
            if (IsRewinding)
                CurRecordingIndex--;
            else
                CurRecordingIndex++;
            
            if(IsRewinding && CurRecordingIndex == 0)
            {
                IsEnd = true;
                RecordObjectInit(RecordType.Default);
                RecordObjectInit(RecordType.Rewind);
            }
            if (!IsRewinding && CurRecordingIndex == totalRecordCount - 1)
            {
                IsRewinding = true;
            }
        }
    }

    private void ExcuteUpdate()
    {
        ExcuteUpdate(RecordType.Default);
        ExcuteUpdate(RecordType.Rewind);
    }

    public void RecordObjectInit(RecordType type)
    {
        List<RecordObject> targetList = GetRecordObjectByType(type);

        for (int i = 0; i < targetList.Count; ++i)
        {
            if (IsEnd)
            {
                targetList[i].InitOnRewind();
                return;
            }
            else
            {
                switch (type)
                {
                    case RecordType.Default:
                        if (IsRewinding)
                            targetList[i].InitOnRewind();
                        else
                            targetList[i].InitOnPlay();
                        break;
                    case RecordType.Rewind:
                        if (IsRewinding)
                            targetList[i].InitOnPlay();
                        else
                            targetList[i].InitOnRewind();
                        break;
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

    public void ApplyData(RecordType type, int index, int diff)
    {
        List<RecordObject> targetList = GetRecordObjectByType(type);

        for (int i = 0; i < targetList.Count; ++i)
        {
            targetList[i].ApplyData(index, diff);
        }
    }

    public void ExcuteUpdate(RecordType type)
    {
        List<RecordObject> targetList = GetRecordObjectByType(type);

        for (int i = 0; i < targetList.Count; ++i)
        {
            if(IsEnd)
                targetList[i].OnRewindUpdate();
            else
            {
                switch (type)
                {
                    case RecordType.Default:
                        if (IsRewinding)
                            targetList[i].OnRewindUpdate();
                        else
                            targetList[i].OnUpdate();
                        break;
                    case RecordType.Rewind:
                        if (IsRewinding)
                            targetList[i].OnUpdate();
                        else
                            targetList[i].OnRewindUpdate();
                        break;
                }
            }
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