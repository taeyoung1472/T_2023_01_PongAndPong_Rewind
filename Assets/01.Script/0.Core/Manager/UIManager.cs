using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingleTon<UIManager>
{
    [Header("[Clock]")]
    [SerializeField] private Slider clockFill;
    [SerializeField] private Image barColor;
    [SerializeField] private TextMeshProUGUI clockTimeText;
    [SerializeField] private Color[] clockColorArray;

    //private int totalTIme { get { return RewindManager.Instance.CurStageRecordCount; } }
    private float totalTIme { get { return RewindManager.Instance.howManySecondsToTrack; } }

    private bool isPause = false;

    public bool IsPause => isPause;

    [SerializeField] private GameObject pauseImg;

    public void Init()
    {
        //RewindManager.Instance.OnTimeChanging += OnTimeChange;
    }

    public void OnPlayTimeChange(float time)
    {
        clockFill.value = time / totalTIme;
        clockTimeText.SetText($"{(int)time}");

        float clockTime = totalTIme / clockColorArray.Length;
        float tempTime = 0;
        for (int i = 0; i < clockColorArray.Length; i++)
        {
            //if (time >= tempTime)
            //{
            //    barColor.color = Color.Lerp(clockColorArray[i], clockColorArray[Mathf.Clamp(i + 1, 0, clockColorArray.Length - 1)], (float)(time % clockTime) / (float)clockTime);
            //}
            tempTime += clockTime;
        }
    }

    public void OnRewindTimeChange(float time)
    {
        clockFill.value = (time / totalTIme);
        clockTimeText.SetText($"{(int)time}");

        float clockTime = totalTIme / clockColorArray.Length;
        float tempTime = totalTIme;
        for (int i = clockColorArray.Length-1; i > 0; i--)
        {
            //if (time < tempTime)
            //{
            //    barColor.color =
            //        Color.Lerp(clockColorArray[i], clockColorArray[Mathf.Clamp(i - 1, 0, clockColorArray.Length - 1)], 
            //        (float)(time % clockTime) / (float)clockTime);
            //}
            tempTime -= clockTime;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !BreakScreenController.Instance.isBreaking)
        {
            if (!isPause && EndManager.Instance.EndPanel.activeSelf == false && EndManager.Instance.NextStagePanel.activeSelf == false)
            {
                isPause = true;
                TimerManager.Instance.ChangeOnTimer(false);
                pauseImg.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                PauseResume();
            }
        }
    }

    public void PauseResume()
    {
        Debug.Log("클릭");
        isPause = false;
        if (StageManager.Instance.GetAreaPlayCheck()) //게임 시작 도중이였을 때
        {
            TimerManager.Instance.ChangeOnTimer(true);
        }
        pauseImg.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    public void PauseSetting()
    {

    }
    public void PauseMenu()
    {
        LoadingSceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
