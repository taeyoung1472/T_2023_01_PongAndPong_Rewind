using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoSingleTon<StageManager>
{
    static StageDataSO stageDataSO;

    [SerializeField] private StageDatabase stageDatabase;
    private StageDataSO curStageDataSO;
    private Stage curStage;
    public Stage CurStage { get { return curStage; } }
    public StageDataSO CurStageDataSO { get { return curStageDataSO; } }

    [SerializeField] private PlayerRewind playerPrefab;
    [SerializeField] private GameObject rewindPlayerPrefab;

    private PlayerRewind playerObj;
    private GameObject rePlayerObj;

    public Image fadeImg;
    private void Awake()
    {
        SpawnStage();
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

    public void SpawnPlayer(Transform spawnPos, bool isDefaultPlayer)
    {
        if (isDefaultPlayer)
            playerObj = Instantiate(playerPrefab, spawnPos.position, Quaternion.identity);
        else
            rePlayerObj = Instantiate(rewindPlayerPrefab, spawnPos.position, Quaternion.identity);
    }

    public void InitPlayer(bool isClear)
    {
        if (isClear)
        {
            playerObj.gameObject.SetActive(false);
            rePlayerObj.SetActive(false);
        }
        else
        {
            playerObj.gameObject.SetActive(false);
            rePlayerObj.SetActive(false);

        }
    }
}
