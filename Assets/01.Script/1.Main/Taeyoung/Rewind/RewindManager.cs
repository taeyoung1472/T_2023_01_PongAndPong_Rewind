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
            // 엔딩
        }
        ExcuteUpdate();
    }

    private void ExcuteUpdate()
    {
        // 끝남
        if (IsEnd)
        {
            // 순행 오브젝트
            for (int i = 0; i < recordObjectList.Count; i++)
            {
                recordObjectList[i].OnRewindUpdate();
            }

            // 역행 오브젝트
            for (int i = 0; i < rewindObjectList.Count; i++)
            {
                rewindObjectList[i].OnRewindUpdate();
            }
        }

        // 순행
        else if (!IsRewinding)
        {
            // 순행 오브젝트
            for (int i = 0; i < recordObjectList.Count; i++)
            {
                recordObjectList[i].OnUpdate();
            }

            // 역행 오브젝트
            for (int i = 0; i < rewindObjectList.Count; i++)
            {
                rewindObjectList[i].OnRewindUpdate();
            }
        }

        // 역행
        else if (IsRewinding)
        {
            // 순행 오브젝트
            for (int i = 0; i < recordObjectList.Count; i++)
            {
                recordObjectList[i].OnRewindUpdate();
            }

            // 역행 오브젝트
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
            // 타이머 초기화
            timer = 0.0f;

            // 역행중
            if (IsRewinding)
            {
                CurRecordingIndex--;

                // 순행 오브젝트
                for (int i = 0; i < recordObjectList.Count; i++)
                {
                    recordObjectList[i].ApplyData(CurRecordingIndex);
                }

                // 역행 오브젝트
                for (int i = 0; i < rewindObjectList.Count; i++)
                {
                    rewindObjectList[i].Recorde((totalRecordCount - 1) - CurRecordingIndex);
                }

                // 게임 끝남
                if (CurRecordingIndex == 0)
                {
                    IsEnd = true;
                    IsRewinding = false;
                }
            }

            // 순행중
            else
            {
                CurRecordingIndex++;

                // 순행 오브젝트
                for (int i = 0; i < recordObjectList.Count; i++)
                {
                    recordObjectList[i].Recorde(CurRecordingIndex);
                }

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
                    PlayerSpawner.Instance.Spawn();
                }
            }
        }
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
