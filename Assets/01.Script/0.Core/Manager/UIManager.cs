using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Rendering;
using UnityEngine.Events;

public class UIManager : MonoSingleTon<UIManager>
{
    [Header("[Clock]")]
    [SerializeField] private Slider clockFill;

    [Header("[FastTime]")]
    [SerializeField] private Image fastTimeImg;
    //[SerializeField] private Toggle fastTimebtn;
    [SerializeField] private bool isFastTime = false;
    [SerializeField] private TextMeshProUGUI fastTimeText;
    private int fastTime = 1;

    private float totalTIme { get { return RewindManager.Instance.howManySecondsToTrack; } }

    private bool isPause = false;

    public bool IsPause => isPause;
    [SerializeField] private FreeLookCamera freeLookCamera;

    [SerializeField] private GameObject timerImg;
    [SerializeField] private GameObject pauseImg;
    [SerializeField] private GameObject collectionImg;

   

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
                timerImg.gameObject.SetActive(false);
                pauseImg.gameObject.SetActive(true);
                freeLookCamera.Rig.transform.position = new Vector3(0f, 3.35f, -13f);
                freeLookCamera._isActivated = false;
                Time.timeScale = 0f;
            }
            else if(!EndManager.Instance.IsEnd)
            {
                PauseResume();
            }
        }

        if(Input.GetKeyDown(KeyCode.Q) && !isPause && !EndManager.Instance.IsEnd)
        {
            if(isFastTime)
            {
                ResetFastForwardTime();
            }
            else
            {
                isFastTime = true;
                FastForwardTime();
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
        Time.timeScale = fastTime;
    }

    public void PauseMenu()
    {
        LoadingSceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void PauseCollection()
    {
        //pauseImg.SetActive(false);
        collectionImg.SetActive(true);
        PhoneCollection.Instance.OnCollectionMenu();
    }

    public void FastForwardTime()
    {
        if (!TimerManager.Instance.isOnTimer || TimerManager.Instance.isRewinding)
            return;

        if (isFastTime)
        {
            fastTime = 2;
            TimerManager.Instance.FastForwardTimeIntensity();
            fastTimeImg.gameObject.SetActive(true);
        }
        else
        {
            fastTime = 1;
            TimerManager.Instance.ResetFastForwardTime();
            fastTimeImg.gameObject.SetActive(false);

        }
        
        
        fastTimeText.SetText($"{fastTime}");
    }
    public void ResetFastForwardTime()
    {
        isFastTime = false;
        FastForwardTime();
    }
}
