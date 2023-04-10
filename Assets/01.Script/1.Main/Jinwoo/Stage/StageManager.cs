using Highlighters;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
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

    public bool isDownButton = false;
    private float reStartCoolTime = 1f;
    private float freelookCoolTime = 2f;
    private bool isRestartPossible = false;

    [Header("카메라 관련")]
    [SerializeField]
    private FreeLookCamera freeLookCam;

    [Header("효과 관련")]
    [SerializeField]
    private ShockWaveController shockWave;
    private void Awake()
    {
        SpawnStage();
        isRestartPossible = false;
    }
    public void Update()
    {
        if (UIManager.Instance.IsPause)
            return;

        if (!isRestartPossible)
        {
            reStartCoolTime -= Time.deltaTime;
            if (reStartCoolTime < 0f)
            {
                isRestartPossible = true;
            }
        }
        if (isDownButton)
        {
            freelookCoolTime -= Time.deltaTime;
            if (freelookCoolTime < 0f)
            {
                isDownButton = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && isRestartPossible && !freeLookCam._isActivated&&
            !BreakScreenController.Instance.isBreaking)
        {
            OnReStartArea();
        }

        if (Input.GetKeyDown(KeyCode.T) && isRestartPossible && !isDownButton)
        {
            isDownButton = true;
            GlitchManager.Instance.CoroutineColorDrift();
            OnFreeLookCam(!freeLookCam._isActivated);
        }

    }
    public void PlayShockWave()
    {
        shockWave.StartShockWave();
    }
    public void OnReStartArea()
    {
        curStage.ReStartArea(true);

        TimerManager.Instance.ChangeOnTimer(false);

        isRestartPossible = false;
        reStartCoolTime = 1f;
    }
    public void OnFreeLookCam(bool isOn)
    {
        if (isOn) //자유시점 온
        {
            freeLookCam.gameObject.SetActive(true);
            freeLookCam.Activate(true);
            if (TimerManager.Instance.isRewinding)
            {
                RewindManager.Instance.StopRewindTimeBySeconds();
            }
                InitPlayer(false);

            TimerManager.Instance.InitTimer();
            TimerManager.Instance.ChangeOnTimer(false);
            TimerManager.Instance.UpdateText();

            RewindManager.Instance.RestartPlay?.Invoke();
        }
        else //자유 시점 오프
        {
            freeLookCam.gameObject.SetActive(false);
            freeLookCam.Activate(false);

            curStage.ReStartArea(false);
        }
        freelookCoolTime = 2f;
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

        OnFreeLookCam(true);
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
            Highlighter highlighter = playerObj.AddComponent<Highlighter>();
            highlighter.GetRenderers();
            highlighter.Renderers.RemoveAt(highlighter.Renderers.Count - 1);
            highlighter.Settings.UseMeshOutline = true;
            highlighter.Settings.MeshOutlineThickness = 0.01f;
            highlighter.Settings.MeshOutlineFront.Color = Color.white;
            
            rePlayerObj = PoolManager.Pop(PoolType.RewindPlayer);
            rePlayerObj.GetComponent<CharacterController>().enabled = false;
            rePlayerObj.transform.position = spawnPos.position;
            rePlayerObj.GetComponent<CharacterController>().enabled = true;

            highlighter = rePlayerObj.AddComponent<Highlighter>();
            highlighter.GetRenderers();
            highlighter.Renderers.RemoveAt(highlighter.Renderers.Count - 1);
            highlighter.Settings.UseMeshOutline = true;
            highlighter.Settings.MeshOutlineThickness = 0.01f;
            highlighter.Settings.MeshOutlineFront.Color = Color.yellow;
        }
        Highlighter.HighlightersNeedReset();

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
