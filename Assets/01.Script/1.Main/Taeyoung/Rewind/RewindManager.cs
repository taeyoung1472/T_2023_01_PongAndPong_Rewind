using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewindManager : MonoSingleTon<RewindManager>
{
    // �÷��� �ð�
    [SerializeField] private int playTime;

    // ��� ����
    [SerializeField] private readonly float recordeTurm = 0.1f;
    public float RecordeTurm { get { return recordeTurm; } }

    // �ð� üũ��
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

    // totalRecord ���
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
            // ����
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

            // Ÿ�̸� �ʱ�ȭ
            timer = Time.time + recordeTurm;

            // ������
            if (IsRewinding)
            {
                CurRecordingIndex--;

                ApplyData(RecordType.Default, CurRecordingIndex);
                RecordData(RecordType.Rewind, (totalRecordCount - 1) - CurRecordingIndex);

                // ���� ����
                if (CurRecordingIndex == 0)
                {
                    checkEnd = true;
                }
            }

            // ������
            else
            {
                CurRecordingIndex++;

                RecordData(RecordType.Default, CurRecordingIndex);
                ApplyData(RecordType.Rewind, (totalRecordCount - 1) - CurRecordingIndex);

                // ���� ����
                if (curRecordingIndex == totalRecordCount - 1)
                {
                    // ���� ����
                    IsRewinding = true;

                    // �ʱ�ȭ
                    for (int i = 0; i < recordObjectList.Count; i++)
                    {
                        recordObjectList[i].InitOnRewind();
                    }

                    Debug.Log("���� ����!");
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
                Debug.LogWarning("Type �� �̻���");
                returnList = null;
                break;
        }

        return returnList;
    }

    public void RegistRecorder(RecordObject recordObject)
    {
        // ������
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