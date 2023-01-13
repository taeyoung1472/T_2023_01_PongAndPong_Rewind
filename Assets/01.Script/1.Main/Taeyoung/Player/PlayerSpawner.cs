using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : RecordObject
{
    [Header("[Ref]")]
    [SerializeField] private PlayerRecord playerPrefab;

    [Header("[Position]")]
    [SerializeField] private Transform defaultPlayerSpawnPos;
    private PlayerRecord defaultPlayer;

    [SerializeField] private Transform rewindPlayerSpawnPos;
    private PlayerRecord rewindPlayer;

    public void Awake()
    {
        Debug.Log("Start");
        Register();
    }

    public override void InitOnPlay()
    {
        Debug.Log("Init");
        if(defaultPlayer == null)
        {
            defaultPlayer = Instantiate(playerPrefab, defaultPlayerSpawnPos.position, Quaternion.identity);
        }
    }

    public override void InitOnRewind()
    {
        if (rewindPlayer == null)
        {
            rewindPlayer = Instantiate(playerPrefab, rewindPlayerSpawnPos.position, Quaternion.identity);
        }
    }
    
    public override void Register()
    {
        RewindManager.Instance.RegistRecorder(this);
    }
    #region Dummy
    public override void Recorde(int index)
    {
    }
    public override void DeRegister()
    {
    }
    public override void ApplyData(int index, int nextIndexDiff)
    {
    }
    #endregion
}
