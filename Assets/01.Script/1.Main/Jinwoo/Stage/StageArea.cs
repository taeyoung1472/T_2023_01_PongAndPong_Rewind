using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Events;

public class StageArea : MonoBehaviour
{
    [Header("기본 데이터")]
    public int stagePlayTime;
    public bool isAreaClear = false;

    [Header("코어 데이터")]
    [SerializeField] private Transform defaultPlayerSpawn;
    [SerializeField] private Transform rewindPlayerSpawn;
    [SerializeField] private Transform endPoint;
    public Transform freeLookCamPos;

    public GameObject gameObjesddct;

    private bool isAreaPlay = false;
    public bool IsAreaPlay { get => isAreaPlay; set => isAreaPlay = value; }

    public UnityEvent areaEndEvent;

    public Action OnEntryArea;
    public Action OnExitArea;

    private void Start()
    {
        isAreaPlay = false;

        defaultPlayerSpawn.gameObject.SetActive(false);
        rewindPlayerSpawn.gameObject.SetActive(false);
        endPoint.gameObject.SetActive(false);
    }
    public void InitArea()
    {
        TimerManager.Instance.InitTimer();
        TimerManager.Instance.SetRewindTime(stagePlayTime + 1);
        //여기서 순환 버퍼 거시기 해주면 될듯 (초기화)
        TimerManager.Instance.ChangeOnTimer(true);
    }
    public void EntryArea(bool isGameStart = false)
    {
        defaultPlayerSpawn.gameObject.SetActive(true);
        rewindPlayerSpawn.gameObject.SetActive(true);
        endPoint.gameObject.SetActive(true);


        //함수 실행 순서 매우 중요;
        IsAreaPlay = true;

        InitArea();
        StageManager.Instance.SpawnPlayer(defaultPlayerSpawn, true);


        //Debug.Log("아리아엔트리");
        if(isGameStart)
            RewindManager.Instance.StartAreaPlay();

        OnEntryArea?.Invoke();
    }

    public void Rewind()
    {
        StageManager.Instance.InitTransform();
        StageManager.Instance.SpawnPlayer(rewindPlayerSpawn, false);
    }

    public void ExitArea()
    {
        OnExitArea?.Invoke();

        defaultPlayerSpawn.gameObject.SetActive(false);
        rewindPlayerSpawn.gameObject.SetActive(false);
        endPoint.gameObject.SetActive(false);

        IsAreaPlay = false;
        if (!isAreaClear) //클리어 못함
        {
            StageManager.Instance.InitPlayer(isAreaClear); //false
            EntryArea(true);
        }
        else //클리어 함
        { 
            StageManager.Instance.InitPlayer(isAreaClear); //true
            areaEndEvent?.Invoke();
        }
    }
}
