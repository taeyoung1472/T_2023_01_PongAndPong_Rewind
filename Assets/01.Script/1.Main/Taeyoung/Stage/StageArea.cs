using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageArea : MonoBehaviour
{
    [SerializeField] private Transform defaultPlayerSpawn;
    [SerializeField] private Transform rewindPlayerSpawn;
    [SerializeField] private int stageTime;

    public int StageTime { get { return stageTime; } }

    private RewindManager rewindManager;

    public void Awake()
    {
        rewindManager = FindObjectOfType<RewindManager>();
    }

    public void EntryArea()
    {
        //rewindManager.Init(this);
    }

    public void ExitArea()
    {

    }
}
