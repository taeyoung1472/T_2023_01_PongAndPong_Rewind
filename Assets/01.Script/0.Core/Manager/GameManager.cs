using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 0;

        RewindManager.playTime = StageWorldSelectData.curStageWorld.stagePlayTime;
        Instantiate(StageWorldSelectData.curStageWorld.StagePrefab);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 1;
            RewindManager.Instance.Init();
        }
    }
}
