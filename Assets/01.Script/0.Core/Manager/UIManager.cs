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
    [SerializeField] private TextMeshProUGUI clockTimeText;

    private float totalTIme { get { return RewindManager.Instance.howManySecondsToTrack; } }

    private bool isPause = false;

    public bool IsPause => isPause;

    [SerializeField] private GameObject pauseImg;

    public void OnPlayTimeChange(float time)
    {
        clockFill.value = time / totalTIme;
        clockTimeText.SetText($"{(int)time}");
    }

    public void OnRewindTimeChange(float time)
    {
        clockFill.value = (time / totalTIme);
        clockTimeText.SetText($"{(int)time}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !BreakScreenController.Instance.isBreaking)
        {
            if (!isPause && !EndManager.Instance.IsEnd)
            {
                isPause = true;
                TimerManager.Instance.ChangeOnTimer(false);
                pauseImg.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
            else if(!EndManager.Instance.IsEnd)
            {
                PauseResume();
            }
        }
    }

    public void PauseResume()
    {
        isPause = false;
        if (StageManager.Instance.GetAreaPlayCheck()) //게임 시작 도중이였을 때
        {
            TimerManager.Instance.ChangeOnTimer(true);
        }
        pauseImg.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PauseMenu()
    {
        LoadingSceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
