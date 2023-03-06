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
        get
        {
            return areaData;
        }

        set
        {
            areaData = value;
        }
    }

    //private int playTime;
    //public int PlayTime { get { return playTime = areaData.stagePlayTime; } }

    //private bool isClear;
    //public bool IsClear { get { return isClear = areaData.isAreaClear; } set { areaData.isAreaClear = value; } }

    [SerializeField] private PlayerReTime playerPrefab;


    private GameObject player;
    private GameObject replayer;

    void Start()
    {

    }
    public void EntryArea(bool isNew = false)
    {
        if (!isNew)
        {
            //RewindManager.Instance.SetArea(this);
        }
        player = Instantiate(playerPrefab, defaultPlayerSpawn.position, Quaternion.identity).gameObject;
        player.GetComponent<PlayerReTime>().Init();
    }

    public void Rewind()
    {
        replayer = Instantiate(playerPrefab, rewindPlayerSpawn.position, Quaternion.identity).gameObject;
        replayer.GetComponent<PlayerReTime>().Init();
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
