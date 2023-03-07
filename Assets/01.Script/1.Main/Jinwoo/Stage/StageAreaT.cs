using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageAreaT : MonoBehaviour
{
    [SerializeField] private Transform defaultPlayerSpawn;
    [SerializeField] private Transform rewindPlayerSpawn;

    [SerializeField]
    private StageAreaDataSO areaData;
    public StageAreaDataSO AreaData
    {
        get { return areaData; }

        set { areaData = value; }
    }

    public int PlayTime { get => AreaData.stagePlayTime; set => AreaData.stagePlayTime = value; }

    public bool IsClear { get { return AreaData.isAreaClear; } set => AreaData.isAreaClear = value; }

    public PlayerReTime playerPrefab;

    public PlayerReTime player;
    public PlayerReTime replayer;

    private bool isRewind = false;
    public bool IsRewind { get => isRewind; set { isRewind = value;  Debug.Log(value); } }


    public void EntryArea(bool isNew = false)
    {
        if (!isNew)
        {
            //RewindManager.Instance.SetArea(this);
            //ExitArea();
        }
        player = Instantiate(playerPrefab, defaultPlayerSpawn.position, Quaternion.identity);
        player.GetComponent<PlayerReTime>().Init();
        player.GetComponent<PlayerReTime>().InitOnPlay();
        PlayerCam.SetTarget(player.transform);
    }

    public void Rewind()
    {
        //Debug.Log("djkfs");
        replayer = Instantiate(playerPrefab, rewindPlayerSpawn.position, Quaternion.identity);
        
        player.GetComponent<PlayerReTime>().InitOnRewind();
        replayer.GetComponent<PlayerReTime>().Init();
        replayer.GetComponent<PlayerReTime>().InitOnPlay();
        PlayerCam.SetTarget(replayer.transform);
    }

    public void ExitArea()
    {
        player.gameObject.SetActive(false);
        if (replayer)
        {
            replayer.gameObject.SetActive(false);
        }
    }
}
