using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleTon<GameManager>
{
    public void LoadMenu()
    {
        LoadingSceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        LoadingSceneManager.LoadScene(1);
    }

    public void LoadTutoLab()
    {
        LoadingSceneManager.LoadScene(11);
    }

    public void LoadNextStage()
    {
        StageManager.stageDataSO = StageManager.stageDataSO.nextStageData;
        LoadingSceneManager.LoadScene(1);
    }

    public void LoadCurrentScene()
    {
        LoadingSceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnApplicationQuit()
    {
        SaveDataManager.Instance.SaveCollectionJSON();
        SaveDataManager.Instance.SaveStageClearJSON();
    }
}
