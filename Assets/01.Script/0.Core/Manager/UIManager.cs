using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Rendering;
using UnityEngine.Events;
using DG.Tweening;

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
    private bool pauseAnimating = false;

    [SerializeField] private GameObject mainImg;
    public GameObject stageImg;
    [SerializeField] private GameObject settingImg;
    [SerializeField] private GameObject collectionImg;
    public GameObject gimmickImg;

   public GameObject[] setActiveFalseObjs;

    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI timeText;

    public GameObject currentOpenImg;

    [SerializeField] private AudioSource fastTimeAudio;
    private float fastTimeAudioVolume = 0f;
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
                if (pauseAnimating)
                    return;

                SetDayText();
                if (isFastTime == true)
                {
                    JinwooVolumeManager.Instance.DisableFastTimeEffect();
                    fastTimeAudioVolume = fastTimeAudio.volume;
                    fastTimeAudio.volume = 0f;
                }
                
                isPause = true;
                TimerManager.Instance.ChangeOnTimer(false);
                timerImg.gameObject.SetActive(false);
                pauseImg.gameObject.SetActive(true);
                pauseImg.transform.DOKill();
                pauseImg.transform.SetLocalPositionAndRotation
                    (new Vector3(0f, -3f, 0f), Quaternion.Euler(0f, 20f, 90f));
                Sequence seq = DOTween.Sequence();
                seq.Append(pauseImg.transform.DORotate(new Vector3(0f, 0f, 0f), 0.5f)).SetUpdate(true);

                freeLookCamera._isCursorVisible = true;

                Time.timeScale = 0f;
            }
            else if(!EndManager.Instance.IsEnd)
            {
                if (isFastTime == true)
                {
                    JinwooVolumeManager.Instance.EnableFastTimeEffect();
                    fastTimeAudio.volume = fastTimeAudioVolume;
                }
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
    public void HomeBtn()
    {
        SetDayText();
        if (currentOpenImg != null)
        {
            BackPressDirector(currentOpenImg.transform);
        }
        mainImg.SetActive(true);
    }
    public void BackBtn()
    {
        SetDayText();
        mainImg.SetActive(true);
        Debug.Log(currentOpenImg);
        BackPressDirector(currentOpenImg.transform);

        setActiveFalseObjs[2].SetActive(true);  

        if(currentOpenImg != null)
        {
            BackPressDirector(currentOpenImg.transform);
        }
        mainImg.SetActive(true);
    }
    public void PauseResume()
    {
        if (pauseAnimating)
            return;
        //freeLookCamera._isActivated = true;
        //timerImg.gameObject.SetActive(true);

        freeLookCamera._isCursorVisible = false;

        isPause = false;
        if (StageManager.Instance.GetAreaPlayCheck()) //게임 시작 도중이였을 때
        {
            TimerManager.Instance.ChangeOnTimer(true);
        }

        if (currentOpenImg != null)
        {
            currentOpenImg.SetActive(false);
        }

        Sequence seq = DOTween.Sequence();
        pauseImg.transform.DOKill();
        pauseAnimating = true;
        seq.Append(pauseImg.transform.DOLocalMoveY(-9f, 0.5f)).SetUpdate(true);
        seq.AppendCallback(() => { 
            pauseImg.gameObject.SetActive(false); 
            pauseAnimating = false; 
        });

        timerImg.gameObject.SetActive(true);

        Time.timeScale = fastTime;
    }

    public void PauseMenu()
    {
        pauseImg.SetActive(false);
        if (currentOpenImg != null)
        {
            currentOpenImg.SetActive(false);
        }

        LoadingSceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void PauseCollection()
    {
        PressDirector(collectionImg.transform);
        PhoneCollection.Instance.OnCollectionMenu();
    }
    public void PauseStage()
    {
        PressDirector(stageImg.transform);
        PhoneStage.Instance.OnStageMenu();
    }

    
    public void PauseSetting()
    {
        PressDirector(settingImg.transform);
    }

    public void PauseGimmick()
    {
        PressDirector(gimmickImg.transform);
        PhoneGimmick.Instance.OnStageGimmick();
    }
   
    public void PressDirector(Transform trm)
    {
        currentOpenImg = trm.gameObject;
        trm.gameObject.SetActive(true);
        trm.DOKill();
        trm.localScale = Vector3.zero + Vector3.forward * 1;
        Sequence seq = DOTween.Sequence();
        seq.Append(trm.DOScaleX(1f, 0.1f)).SetUpdate(true);
        seq.Join(trm.DOScaleY(1f, 0.2f)).SetUpdate(true);
    }
    public void BackPressDirector(Transform trm)
    {
        Debug.Log(currentOpenImg);

        SetDayText();
        trm.DOKill();
        trm.localScale = Vector2.one;
        Sequence seq = DOTween.Sequence();
        seq.Append(trm.DOScaleY(0f, 0.2f)).SetUpdate(true);
        seq.Insert(0.1f, trm.DOScaleX(0f, 0.1f)).SetUpdate(true);
        seq.AppendCallback(() =>
        {
            trm.gameObject.SetActive(false);
        });
    }

    public void SetDayText()
    { 
        dayText.SetText(System.DateTime.Now.ToString(("MM\ndd")));
        timeText.SetText(System.DateTime.Now.ToString(("yyyy-MM-dd")));
    }
    public void FastForwardTime()
    {
        if (!TimerManager.Instance.isOnTimer || TimerManager.Instance.isRewinding)
            return;

        if (isFastTime)
        {
            fastTimeAudio.Play();
            fastTime = 3;
            TimerManager.Instance.FastForwardTimeIntensity();
            fastTimeImg.gameObject.SetActive(true);
        }
        else
        {
            fastTimeAudio.Stop();
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
