using UnityEngine;

public class StageArea : MonoBehaviour
{
    [Header("기본 데이터")]
    public int stagePlayTime;
    public bool isAreaClear = false;

    [Header("코어 데이터")]
    [SerializeField] private Transform defaultPlayerSpawn;
    [SerializeField] private Transform rewindPlayerSpawn;

    public GameObject gameObjesddct;

    private bool isAreaPlay = false;
    public bool IsAreaPlay { get => isAreaPlay; set => isAreaPlay = value; }
    private void Start()
    {
        isAreaPlay = false;
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
        //함수 실행 순서 매우 중요;
        IsAreaPlay = true;

        InitArea();
        StageManager.Instance.SpawnPlayer(defaultPlayerSpawn, true);


        Debug.Log("아리아엔트리");
        if(isGameStart)
            RewindManager.Instance.StartAreaPlay();
    }

    public void Rewind()
    {
        StageManager.Instance.InitTransform();
        StageManager.Instance.SpawnPlayer(rewindPlayerSpawn, false);
    }

    public void ExitArea()
    {
        IsAreaPlay = false;
        if (!isAreaClear)
        {
            StageManager.Instance.InitPlayer(isAreaClear);
            EntryArea(true);
        }
        else
        {
            StageManager.Instance.InitPlayer(isAreaClear);

        }
    }
}
