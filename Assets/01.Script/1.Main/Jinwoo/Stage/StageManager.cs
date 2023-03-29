using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoSingleTon<StageManager>
{
    [Header("스테이지 관련")]
    public static StageDataSO stageDataSO;

    //[SerializeField] private StageDatabase stageDatabase;
    private StageDataSO curStageDataSO;
    public StageDataSO CurStageDataSO { get { return curStageDataSO; } }

    private Stage curStage;
    public Stage CurStage { get { return curStage; } }

    [Header("플레이어 관련")]
    //[SerializeField] private PlayerRewind playerPrefab;
    //[SerializeField] private GameObject rewindPlayerPrefab;

    private GameObject playerObj;
    private GameObject rePlayerObj;

    public Image fadeImg;

    private float reStartCoolTime = 1f;
    private bool isRestartPossible = false;

    private void Awake()
    {
        SpawnStage();
        isRestartPossible = false;
    }
    public void Update()
    {
        if (!isRestartPossible)
        {
            reStartCoolTime -= Time.deltaTime;
            if (reStartCoolTime < 0f)
            {
                isRestartPossible = true;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.R) && isRestartPossible)
        {
            RewindManager.Instance.RestartPlay?.Invoke();
            curStage.ReStartArea();
            isRestartPossible = false;
            reStartCoolTime = 1f;
        }

    }
    public StageArea GetCurArea()
    {
        if (CurStage == null)
        {
            return null;
        }
        else
        {
            return CurStage.curArea;
        }
    }

    public void SpawnStage()
    {
        curStageDataSO = stageDataSO;

        curStage = Instantiate(curStageDataSO.stagePrefab, Vector3.zero, Quaternion.identity);
        curStage.Init();
    }
    public void NextStage()
    {

    }

    public void SpawnPlayer(Transform spawnPos, bool isDefaultPlayer, bool isFirst = false)
    {

        if (isDefaultPlayer)
        {
            //playerObj = Instantiate(playerPrefab, spawnPos.position, Quaternion.identity);
            playerObj = PoolManager.Pop(PoolType.Player);
            playerObj.GetComponent<CharacterController>().enabled = false;
            playerObj.transform.position = spawnPos.position;
            playerObj.GetComponent<CharacterController>().enabled = true;
            //TestParticleSpawn.Instance.playerPos = playerObj.transform;
        }
        else
        {
            rePlayerObj = PoolManager.Pop(PoolType.RewindPlayer);
            rePlayerObj.GetComponent<CharacterController>().enabled = false;
            rePlayerObj.transform.position = spawnPos.position;
            rePlayerObj.GetComponent<CharacterController>().enabled = true;
        }
            
            //rePlayerObj = Instantiate(rewindPlayerPrefab, spawnPos.position, Quaternion.identity);
                    
    }

    public void InitPlayer(bool isClear)
    {
        if(rePlayerObj != null)
        {
            PoolManager.Push(PoolType.RewindPlayer, rePlayerObj);
        }
        if (playerObj != null)
        {
            PoolManager.Push(PoolType.Player, playerObj);
        }


        //playerObj.gameObject.SetActive(false);
        //rePlayerObj.SetActive(false);


        //if (isClear)
        //{
        //    playerObj.gameObject.SetActive(false);
        //    rePlayerObj.SetActive(false);
        //}
        //else
        //{
        //    playerObj.gameObject.SetActive(false);
        //    rePlayerObj.SetActive(false);

        //}
    }
}
