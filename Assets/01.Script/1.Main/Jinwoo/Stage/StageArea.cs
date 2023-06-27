using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Events;

public class StageArea : MonoBehaviour
{
    [Header("�⺻ ������")]
    public int stagePlayTime;
    public bool isAreaClear = false;

    [Header("�ھ� ������")]
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
        //���⼭ ��ȯ ���� �Žñ� ���ָ� �ɵ� (�ʱ�ȭ)
        TimerManager.Instance.ChangeOnTimer(true);
    }
    public void EntryArea(bool isGameStart = false)
    {
        defaultPlayerSpawn.gameObject.SetActive(true);
        rewindPlayerSpawn.gameObject.SetActive(true);
        endPoint.gameObject.SetActive(true);


        //�Լ� ���� ���� �ſ� �߿�;
        IsAreaPlay = true;

        InitArea();
        StageManager.Instance.SpawnPlayer(defaultPlayerSpawn, true);


        //Debug.Log("�Ƹ��ƿ�Ʈ��");
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
        if (!isAreaClear) //Ŭ���� ����
        {
            StageManager.Instance.InitPlayer(isAreaClear); //false
            EntryArea(true);
        }
        else //Ŭ���� ��
        { 
            StageManager.Instance.InitPlayer(isAreaClear); //true
            areaEndEvent?.Invoke();
        }
    }
}
