using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RewindManager : MonoSingleTon<RewindManager>
{
    // �÷��� �ð�
    private int maxPlayTime;
    private int curStagePlayTime;

    // ��� ����
    private readonly float recordeTurm = 0.1f;

    public float RecordeTurm { get { return recordeTurm; } }

    // �ð� üũ��
    private float timer = 0.0f;

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

            // �м�
            if (IsEnd)
            {
                ApplyData(RecordType.Default, defaultIndex, diff);
                ApplyData(RecordType.Rewind, rewindIndex, -diff);
            }

            // ����
            else if (!IsRewinding)
            {
                RecordData(RecordType.Default, defaultIndex);
                ApplyData(RecordType.Rewind, rewindIndex, diff);
            }

            // ����
            else
            {
                RecordData(RecordType.Rewind, rewindIndex);
                ApplyData(RecordType.Default, defaultIndex, diff);
            }
        }
    }

    private int totalRecordCount;
    public int TotalRecordCount 
    { 
        get 
        {
            if (totalRecordCount == 0) 
                totalRecordCount = Mathf.RoundToInt(maxPlayTime / recordeTurm); 
            
            return totalRecordCount; 
        } 
    }

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
    public bool IsEnd { get { return isEnd; } set { isEnd = value; EndManager.Instance.ActivePanel(); Time.timeScale = 1; } }

    // Event
    public Action<int> OnTimeChanging;

    // RecordObjectList
    private List<RecordObject> recordObjectList = new();
    private List<RecordObject> rewindObjectList = new();

    [ContextMenu("Init")]
    public void Init()
    {
        isInit = true;
        StageArea[] areas = FindObjectsOfType<StageArea>();
        RecordObject[] recordObjects = FindObjectsOfType<RecordObject>();

        foreach (var area in areas)
        {
            if(area.PlayTime > maxPlayTime)
            {
                maxPlayTime = area.PlayTime;
            }
        }

        foreach (var obj in recordObjects)
        {
            if (obj.IsRewind)
            {
                rewindObjectList.Add(obj);
            }
            else
            {
                recordObjectList.Add(obj);
            }
            obj.Init();
        }

        UIManager.Instance.Init();

        timer = Time.time + recordeTurm;

        IsRewinding = false;
    }

    public void SetArea(StageArea area)
    {
        curStageArea = area;
        curStagePlayTime = curStageArea.PlayTime;
    }

    public void Update()
    {
        if (!isInit)
            return;

        ExcuteUpdate();
        LifeCycle();
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

            if (IsRewinding && CurRecordingIndex == 0)
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
            if (IsEnd)
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
                Debug.LogWarning("Type �� �̻���");
                returnList = null;
                break;
        }

        return returnList;
    }
}

public enum RecordType
{
    Default,
    Rewind,
    End,
}