using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleTon<UIManager>
{
    [Header("[Clock]")]
    [SerializeField] private Slider clockFill;
    [SerializeField] private TextMeshProUGUI clockTimeText;

    private float totalTIme { get { return RewindManager.Instance.howManySecondsToTrack; } }

    private bool isPause = false;

    public bool IsPause => isPause;

    [SerializeField] private GameObject pauseImg;

    private void Awake()
    {
        TimerManager.Instance.OnTimeChange += OnTimeChange;
    }

    public void OnTimeChange(float time)
    {
        clockFill.value = time / totalTIme;
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
            else if (!EndManager.Instance.IsEnd)
            {
                PauseResume();
            }
        }
    }

    public void PauseResume()
    {
        isPause = false;
        if (StageManager.Instance.GetAreaPlayCheck()) //���� ���� �����̿��� ��
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
