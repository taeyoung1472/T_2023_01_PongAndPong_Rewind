using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private GameObject pauseUi;

    [SerializeField] private StageData testStage;//StageDataSO testStage;

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    pauseUi.SetActive(!pauseUi.activeSelf);
        //    if (pauseUi.activeSelf)
        //        Time.timeScale = 0;
        //    else
        //        Time.timeScale = 1;
        //}
    }

    public void ClosePausePanel()
    {
        //pauseUi.SetActive(false);
        //Time.timeScale = 1;
    }

    public void GameClear()
    {
        LoadingSceneManager.LoadScene(1);
    }

    public void ReplayGame()
    {
        LoadingSceneManager.LoadScene(2);
    }
}
