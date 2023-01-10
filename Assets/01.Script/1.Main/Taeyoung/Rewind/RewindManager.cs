using System;
using System.Collections.Generic;
using UnityEngine;

public class RewindManager : MonoSingleTon<RewindManager>
{
    [SerializeField] private int playTime;
    [SerializeField] private float recordeTurm = 0.1f;
    public float RecordeTurm { get { return recordeTurm; } }

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
    public float CurRecordingPercent { get { return Mathf.Clamp(timer / recordeTurm, 0, 1); } }

    // Logic
    private bool isRewinding = false;
    public bool IsRewinding { get { return isRewinding; } set { isRewinding = value; } }
    private bool isEnd = false;
    public bool IsEnd { get { return isEnd; } set { isEnd = value; } }

    // Event
    public Action<int> OnTimeChanging;

    // RecordObjectList
    private List<RecordObject> recordObjectList = new();
    private List<RecordObject> rewindObjectList = new();

    public void OnValidate()
    {
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
            timer += Time.deltaTime;
            // ����
        }
        ExcuteUpdate();
    }

    private void ExcuteUpdate()
    {
        // ����
        if (IsEnd)
        {
            // ���� ������Ʈ
            for (int i = 0; i < recordObjectList.Count; i++)
            {
                recordObjectList[i].OnRewindUpdate();
            }

            // ���� ������Ʈ
            for (int i = 0; i < rewindObjectList.Count; i++)
            {
                rewindObjectList[i].OnRewindUpdate();
            }
        }

        // ����
        else if (!IsRewinding)
        {
            // ���� ������Ʈ
            for (int i = 0; i < recordObjectList.Count; i++)
            {
                recordObjectList[i].OnUpdate();
            }

            // ���� ������Ʈ
            for (int i = 0; i < rewindObjectList.Count; i++)
            {
                rewindObjectList[i].OnRewindUpdate();
            }
        }

        // ����
        else if (IsRewinding)
        {
            // ���� ������Ʈ
            for (int i = 0; i < recordObjectList.Count; i++)
            {
                recordObjectList[i].OnRewindUpdate();
            }

            // ���� ������Ʈ
            for (int i = 0; i < rewindObjectList.Count; i++)
            {
                rewindObjectList[i].OnUpdate();
            }
        }
    }

    private void Recorde()
    {
        timer += Time.deltaTime;

        if (timer > recordeTurm)
        {
            // Ÿ�̸� �ʱ�ȭ
            timer = 0.0f;

            // ������
            if (IsRewinding)
            {
                CurRecordingIndex--;

                // ���� ������Ʈ
                for (int i = 0; i < recordObjectList.Count; i++)
                {
                    recordObjectList[i].ApplyData(CurRecordingIndex);
                }

                // ���� ������Ʈ
                for (int i = 0; i < rewindObjectList.Count; i++)
                {
                    rewindObjectList[i].Recorde((totalRecordCount - 1) - CurRecordingIndex);
                }

                // ���� ����
                if (CurRecordingIndex == 0)
                {
                    IsEnd = true;
                    IsRewinding = false;
                }
            }

            // ������
            else
            {
                CurRecordingIndex++;

                // ���� ������Ʈ
                for (int i = 0; i < recordObjectList.Count; i++)
                {
                    recordObjectList[i].Recorde(CurRecordingIndex);
                }

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
                    PlayerSpawner.Instance.Spawn();
                }
            }
        }
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
