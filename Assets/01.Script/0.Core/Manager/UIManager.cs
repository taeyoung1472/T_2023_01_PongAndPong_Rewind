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

    [SerializeField] private GameObject mainImg;
    [SerializeField] private GameObject stageImg;
    [SerializeField] private GameObject collectionImg;
    [SerializeField] private GameObject gimmickImg;

    [SerializeField] private GameObject[] setActiveFalseObjs;

    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private GameObject currentOpenImg;
    
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
                SetDayText();

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
    public void HomeBtn()
    {
        SetDayText();
        BackPressDirector(currentOpenImg.transform);
        mainImg.SetActive(true);
    }
    public void BackBtn()
    {
        SetDayText();
        mainImg.SetActive(true);
        BackPressDirector(currentOpenImg.transform);
        if (setActiveFalseObjs[2].activeSelf)
        {
            //머머머ㅓㅁ
        }
        else
        {
            mainImg.SetActive(true);
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
        PressDirector(collectionImg.transform);
        PhoneCollection.Instance.OnCollectionMenu();
    }
    public void PauseStage()
    {
        PressDirector(stageImg.transform);
        PhoneStage.Instance.OnStageMenu();
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
        trm.localScale = Vector2.zero;
        Sequence seq = DOTween.Sequence();
        seq.Append(trm.DOScaleX(1f, 0.1f)).SetUpdate(true);
        seq.Join(trm.DOScaleY(1f, 0.2f)).SetUpdate(true);
    }
    public void BackPressDirector(Transform trm)
    {
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
            fastTime = 3;
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
