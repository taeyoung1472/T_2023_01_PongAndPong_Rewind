using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageArea : MonoBehaviour
{
    [SerializeField]
    private StageAreaDataSO areaData;
    public StageAreaDataSO AreaData { get => areaData; set => areaData = value; }
    

    [SerializeField] private Transform defaultPlayerSpawn;
    [SerializeField] private Transform rewindPlayerSpawn;
    public void Init()
    {
        TimerManager.Instance.InitTimer();
        TimerManager.Instance.SetRewindTime(AreaData.stagePlayTime+1);
        //여기서 순환 버퍼 거시기 해주면 될듯 (초기화)
        TimerManager.Instance.isOnTimer = true;
    }
    public void EntryArea(bool isNew = false)
    {
        Init();
        StageManager.Instance.SpawnPlayer(defaultPlayerSpawn, true);
    }

    public void Rewind()
    {
        StageManager.Instance.SpawnPlayer(rewindPlayerSpawn, false);

    }

    public void ExitArea()
    {
        if(!areaData.isAreaClear)
        {
            StageManager.Instance.InitPlayer(AreaData.isAreaClear);
            EntryArea(false);
        }
        else
        {
            StageManager.Instance.InitPlayer(AreaData.isAreaClear);

        }
    }
}
