using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleTon<GameManager>
{
    public void StagePlayStart()
    {

    }
    public void StagePlayEnd()
    {

    }

    public void ClosePausePanel()
    {

    }

    public void LoadMenu()
    {
        LoadingSceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        LoadingSceneManager.LoadScene(1);
    }

    public void LoadNextStage()
    {
        StageManager.stageDataSO = StageManager.stageDataSO.nextStageData;
        LoadingSceneManager.LoadScene(1);
    }
}
