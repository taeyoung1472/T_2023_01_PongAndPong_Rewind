using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPlayManager : MonoBehaviour
{
    [Header("스테이지 관련")]
    public static StageDataSO stageDataSO;
    [SerializeField] private StageDataSO demoStageDataSO;

    private StageDataSO curStageDataSO;
    public StageDataSO CurStageDataSO { get { return curStageDataSO; } }

    private Stage curStage;
    public Stage CurStage { get { return curStage; } }

    private GameObject playerObj;
    private GameObject rePlayerObj;

    public Image fadeImg;

    [HideInInspector] public bool isGameStart = false;
    [HideInInspector] public bool isDownButton = false;
    private float reStartCoolTime = 1f;
    private float freelookCoolTime = 2f;
    private bool isRestartPossible = false;

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

        if (Input.GetKeyDown(KeyCode.R) && isRestartPossible &&
            !BreakScreenController.Instance.isBreaking && !EndManager.Instance.IsEnd)
        {
            OnReStartArea();
        }
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

        TimerManager.Instance.ChangeOnTimer(false);

        isRestartPossible = false;
        reStartCoolTime = 1f;
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
    }

    public void InitTransform()
    {
        CurStage.transform.DOKill();
    }

    public void SpawnPlayer(Transform spawnPos, bool isDefaultPlayer, bool isFirst = false)
    {

        if (isDefaultPlayer)
        {
            playerObj = PoolManager.Pop(PoolType.Player);
            if (rePlayerObj != null)
                rePlayerObj.GetComponent<Player>().DisableReset();
            playerObj.GetComponent<Player>().EnableReset();
            playerObj.transform.position = spawnPos.position;
        }
        else
        {
            rePlayerObj = PoolManager.Pop(PoolType.RewindPlayer);
            if (playerObj != null)
                playerObj.GetComponent<Player>().DisableReset();
            rePlayerObj.GetComponent<Player>().EnableReset();
            rePlayerObj.transform.position = spawnPos.position;
        }
    }

    public void InitPlayer(bool isClear)
    {
        if (rePlayerObj != null)
        {
            PoolManager.Push(PoolType.RewindPlayer, rePlayerObj);
        }
        if (playerObj != null)
        {
            PoolManager.Push(PoolType.Player, playerObj);
        }
    }

    public GameObject GetCurrentPlayer()
    {
        if (TimerManager.Instance.isRewinding)
        {
            return rePlayerObj;
        }
        else
        {
            return playerObj;
        }
    }
}
