using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageAreaT : MonoBehaviour
{
    #region º¯¼öµé
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

    //public PlayerRewindTest playerPrefab;

    public PlayerRewindTest player;
    public GameObject replayer;
    public PlayerRewindTest p;
    public GameObject p2;

    private bool isRewind = false;
    public bool IsRewind { get => isRewind; set { isRewind = value;  } }

    #endregion
    private void Start()
    {
        RewindTestManager.Instance.ReTimeStop.AddListener(ExitArea);
    }

    public void EntryArea(bool isNew = false)
    {
        if (!isNew)
        {
            //RewindManager.Instance.SetArea(this);
            //ExitArea();
        }
        RewindTestManager.Instance.howManySecondsToTrack = PlayTime;
        p = Instantiate(player, defaultPlayerSpawn.position, Quaternion.identity);
        
        //PlayerCam.SetTarget(player.transform);
    }

    public void Rewind()
    {
        //Debug.Log("djkfs");
        p2 =Instantiate(replayer, rewindPlayerSpawn.position, Quaternion.identity);
        
        //PlayerCam.SetTarget(replayer.transform);
    }

    public void ExitArea()
    {
        p.gameObject.SetActive(false);
        if (p2)
        {
            p2.gameObject.SetActive(false);
        }
    }
}
