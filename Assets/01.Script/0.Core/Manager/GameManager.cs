using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleTon<GameManager>
{
    //[SerializeField] private GameObject pauseUi;

    //[SerializeField] private StageData testStage;//StageDataSO testStage;

    //public void Update()
    //{
    //    //if (Input.GetKeyDown(KeyCode.Escape))
    //    //{
    //    //    pauseUi.SetActive(!pauseUi.activeSelf);
    //    //    if (pauseUi.activeSelf)
    //    //        Time.timeScale = 0;
    //    //    else
    //    //        Time.timeScale = 1;
    //    //}
    //}
    

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
        SceneManager.LoadScene(0);//LoadingSceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
