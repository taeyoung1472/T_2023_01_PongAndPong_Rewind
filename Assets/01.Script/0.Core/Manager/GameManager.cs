using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject infoUi;
    [SerializeField] private GameObject pauseUi;

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
            infoUi.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !infoUi.activeSelf)
        {
            pauseUi.SetActive(!pauseUi.activeSelf);
            if (pauseUi.activeSelf)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
    }

    public void ClosePausePanel()
    {
        pauseUi.SetActive(false);
        Time.timeScale = 1;
    }

    public void GameClear()
    {
        StageWorldSelectData.curStageWorld.isClear = true;
        LoadingSceneManager.LoadScene(1);
    }

    public void ReplayGame()
    {
        LoadingSceneManager.LoadScene(2);
    }
}
