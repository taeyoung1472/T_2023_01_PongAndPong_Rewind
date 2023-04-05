using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingleTon<UIManager>
{
    [Header("[Clock]")]
    [SerializeField] private Image clockFill;
    [SerializeField] private TextMeshProUGUI clockTimeText;
    [SerializeField] private Color[] clockColorArray;

    //private int totalTIme { get { return RewindManager.Instance.CurStageRecordCount; } }
    private int totalTIme { get { return (int)RewindManager.Instance.howManySecondsToTrack -1; } }

    private bool isPause = false;

    public bool IsPause => isPause;

    [SerializeField] private Image pauseImg;

    public void Init()
    {
        //RewindManager.Instance.OnTimeChanging += OnTimeChange;
        
    }

    public void OnPlayTimeChange(int time)
    {
        clockFill.fillAmount = time / (float)(totalTIme);
        clockTimeText.SetText($"{time}");

        int clockTime = totalTIme / clockColorArray.Length;
        int tempTime = 0;
        for (int i = 0; i < clockColorArray.Length; i++)
        {
            if (time >= tempTime)
            {
                clockFill.color = Color.Lerp(clockColorArray[i], clockColorArray[Mathf.Clamp(i + 1, 0, clockColorArray.Length - 1)], (float)(time % clockTime) / (float)clockTime);
            }
            tempTime += clockTime;
        }
    }

    public void OnRewindTimeChange(int time)
    {
        clockFill.fillAmount = (time / (float)(totalTIme));
        clockTimeText.SetText($"{time}");

        int clockTime = totalTIme / clockColorArray.Length;
        int tempTime = totalTIme;
        for (int i = clockColorArray.Length -1; i > 0; i--)
        {
            if (time <= tempTime)
            {
                clockFill.color =
                    Color.Lerp(clockColorArray[i], clockColorArray[Mathf.Clamp(i - 1, 0, clockColorArray.Length - 1)], 
                    (float)(time % clockTime) / (float)clockTime);
            }
            tempTime -= clockTime;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !BreakScreenController.Instance.isBreaking)
        {
            if (!isPause)
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
        Debug.Log("Å¬¸¯");
        isPause = false;
        TimerManager.Instance.ChangeOnTimer(true);
        pauseImg.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    public void PauseSetting()
    {

    }
    public void PauseMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
