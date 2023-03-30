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
    [SerializeField] private PlayerRewind playerPrefab;
    [SerializeField] private GameObject rewindPlayerPrefab;

    private PlayerRewind playerObj;
    private GameObject rePlayerObj;

    public Image fadeImg;
    private void Awake()
    {
        SpawnStage();
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
            playerObj = Instantiate(playerPrefab, spawnPos.position, Quaternion.identity);
            TestParticleSpawn.Instance.playerPos = playerObj.transform;
        }
        else
            rePlayerObj = Instantiate(rewindPlayerPrefab, spawnPos.position, Quaternion.identity);

        //if (isFirst)
        //{
        //    if (isDefaultPlayer)
        //        playerObj = Instantiate(playerPrefab, spawnPos.position, Quaternion.identity);
        //    else
        //        rePlayerObj = Instantiate(rewindPlayerPrefab, spawnPos.position, Quaternion.identity);
        //}
        //else
        //{
        //    if (isDefaultPlayer)
        //    {
        //        playerObj.gameObject.SetActive(true);
        //        playerObj.transform.position = spawnPos.position;
        //    }
        //    else
        //    {
        //        rePlayerObj.gameObject.SetActive(true);
        //        rePlayerObj.transform.position = spawnPos.position;
        //    }
        //}

    }

    public void InitPlayer(bool isClear)
    {
        
        playerObj.gameObject.SetActive(false);
        rePlayerObj.SetActive(false);
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
