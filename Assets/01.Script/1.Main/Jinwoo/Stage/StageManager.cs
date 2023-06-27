using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class StageManager : MonoSingleTon<StageManager>
{
    [Header("�������� ����")]
    public static StageDataSO stageDataSO;
    [SerializeField] private StageDataSO demoStageDataSO;

    //[SerializeField] private StageDatabase stageDatabase;
    private StageDataSO curStageDataSO;
    public StageDataSO CurStageDataSO { get { return curStageDataSO; } }

    private Stage curStage;
    public Stage CurStage { get { return curStage; } }

    [Header("�÷��̾� ����")]
    //[SerializeField] private PlayerRewind playerPrefab;
    //[SerializeField] private GameObject rewindPlayerPrefab;

    private GameObject playerObj; //ó�� �÷����ϴ� �÷��̾�
    private GameObject rePlayerObj; // ���� �� �����ϴ� �÷��̾�

    public Image fadeImg;

    [HideInInspector] public bool isGameStart = false;
    [HideInInspector] public bool isDownButton = false;
    private float reStartCoolTime = 1f;
    private float freelookCoolTime = 1f;
    private bool isRestartPossible = false;
    private bool inputLock = false;
    public bool InputLock { get => inputLock; set => inputLock = value; }

    [Header("ī�޶� ����")]
    [SerializeField]
    private FreeLookCamera freeLookCam;

    [Header("ȿ�� ����")]
    [SerializeField]
    private ShockWaveController shockWave;

    private void Awake()
    {
        SpawnStage();
        isRestartPossible = false;

        reStartCoolTime = 1f;
        freelookCoolTime = 1f;
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

        if (Input.GetKeyDown(KeyCode.R) && isRestartPossible && !freeLookCam._isActivated &&
            !BreakScreenController.Instance.isBreaking && !EndManager.Instance.IsEnd && !inputLock)
        {
            OnReStartArea();
        }

        if (Input.GetKeyDown(KeyCode.T) && isRestartPossible && !isDownButton && !EndManager.Instance.IsEnd && !inputLock)
        {
            isDownButton = true;
            GlitchManager.Instance.CoroutineColorDrift();
            OnFreeLookCam(!freeLookCam._isActivated);
        }

    }

    public void SetFreeLookCamInitPos(Transform camPos)
    {
        freeLookCam.InitPosCam(camPos);
    }
    public void SetAreaPlay(bool isPlay)
    {
        curStage.curArea.IsAreaPlay = isPlay;
    }
    public bool GetAreaPlayCheck()
    {
        return CurStage.curArea.IsAreaPlay;
    }
    public void PlayShockWave()
    {
        shockWave.StartShockWave();
    }
    public void OnReStartArea()
    {
        curStage.ReStartArea(true);

        UIManager.Instance.ResetFastForwardTime();
        TimerManager.Instance.ChangeOnTimer(false);

        isRestartPossible = false;
        reStartCoolTime = 1f;

        SaveDataManager.Instance.LoadCollectionJSON();


    }
    public void OnFreeLookCam(bool isOn)
    {
        if (UIManager.Instance.IsPause)
            return;

        AudioManager.PlayAudio(SoundType.OnGameStart);
        Debug.Log(isOn);
        
        if (isOn) //�������� ��
        {
            SetAreaPlay(false);
            freeLookCam.gameObject.SetActive(true);
            freeLookCam.Activate(true);
            if (TimerManager.Instance.isRewinding)
            {
                RewindManager.Instance.StopRewindTimeBySeconds();
            }
            InitPlayer(false);

            UIManager.Instance.ResetFastForwardTime();

            TimerManager.Instance.InitTimer();
            TimerManager.Instance.ChangeOnTimer(false);
            TimerManager.Instance.UpdateText();

            RewindManager.Instance.RestartPlay?.Invoke();
        }
        else //���� ���� ����
        {
            freeLookCam.gameObject.SetActive(false);
            freeLookCam.Activate(false);

            curStage.ReStartArea(false);
        }
        freelookCoolTime = 2f;
        CamManager.Instance?.TargetGroupReset();
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
        if (stageDataSO == null)
        {
            stageDataSO = demoStageDataSO;
        }
        curStageDataSO = stageDataSO;

        curStage = Instantiate(curStageDataSO.stagePrefab, Vector3.zero, Quaternion.identity);
        
        curStage.Init();

        OnFreeLookCam(true);
    }
    public void NextStage()
    {

    }
    public void InitTransform()
    {
        CurStage.transform.DOKill();
        // CurStage.transform.rotation = Quaternion.Euler(0, 0, 0);

    }
    public void SpawnPlayer(Transform spawnPos, bool isDefaultPlayer, bool isFirst = false)
    {

        if (isDefaultPlayer)
        {
            //playerObj = Instantiate(playerPrefab, spawnPos.position, Quaternion.identity);
            playerObj = PoolManager.Pop(PoolType.Player);
            //if (rePlayerObj != null)
            //    rePlayerObj.GetComponent<Player>().DisableReset();
            playerObj.GetComponent<Player>().EnableReset();
            playerObj.transform.position = spawnPos.position;
            //TestParticleSpawn.Instance.playerPos = playerObj.transform;
        }
        else
        {
            rePlayerObj = PoolManager.Pop(PoolType.RewindPlayer);
            //if (playerObj != null)
            //    playerObj.GetComponent<Player>().DisableReset();
            rePlayerObj.GetComponent<Player>().EnableReset();
            rePlayerObj.transform.position = spawnPos.position;
        }
    }

    public void InitPlayer(bool isClear)
    {
        if (rePlayerObj != null)
        {
            rePlayerObj.GetComponent<Player>().DisableReset();
            PoolManager.Push(PoolType.RewindPlayer, rePlayerObj);
        }
        if (playerObj != null)
        {
            playerObj.GetComponent<Player>().DisableReset();
            PoolManager.Push(PoolType.Player, playerObj);
        }

    }

    public GameObject GetCurrentPlayer()
    {
        if (TimerManager.Instance.isRewinding)
        {
            Debug.Log("��� ��ȯ");
            return rePlayerObj;
        }
        else
        {
            return playerObj;
        }
    }
}
