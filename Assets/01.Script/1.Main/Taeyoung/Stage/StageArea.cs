using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageArea : MonoBehaviour
{
    [SerializeField] private Transform defaultPlayerSpawn;
    [SerializeField] private Transform rewindPlayerSpawn;

    [SerializeField] private Transform rightTop, leftBottom;

    [SerializeField] private int playTime;

    [SerializeField] private PlayerRecord playerPrefab;

    private bool isClear;
    public bool IsClear { get { return isClear; } set { isClear = value; } }

    public int PlayTime { get { return playTime; } }

    private RewindManager rewindManager;

    private GameObject player;
    private GameObject replayer;

    public void Awake()
    {
        rewindManager = FindObjectOfType<RewindManager>();
    }

    public void EntryArea(bool isNew = false)
    {
        StageCamera.Instance.SetCameraFov(rightTop, leftBottom);
        if (!isNew)
        {
            RewindManager.Instance.SetArea(this);
        }
        player = Instantiate(playerPrefab, defaultPlayerSpawn.position, Quaternion.identity).gameObject;
        player.GetComponent<PlayerRecord>().Init();
    }

    public void Rewind()
    {
        replayer = Instantiate(playerPrefab, rewindPlayerSpawn.position, Quaternion.identity).gameObject;
        replayer.GetComponent<PlayerRecord>().Init();
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
