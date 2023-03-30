using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TimerManager : MonoSingleTon<TimerManager>
{
    #region 변수들
    [SerializeField] private TextMeshProUGUI timeText;
    public float CurrentTimer { get; set; }


    private float rewindingTime = 0; 
    public float RewindingTime
    {
        get
        {
            //Debug.Log(rewindingTime);
            return rewindingTime;
        }
        set
        {
            rewindingTime = value;
        }
    }

    public bool isOnTimer = false;


    private bool isRewindStart = false;
    public bool isRewinding = false;

    [SerializeField] private float rewindIntensity = 0.01f;          //되감기 속도를 변경하는 변수
    private float rewindValue = 0;

    [Header("[Volume]")]
    [SerializeField] private GameObject defaultVolume;
    [SerializeField] private GameObject rewindVolume;

    public UnityEvent startRewind;
    #endregion
    private void Awake()
    {
        //InitTimer();
    }
    public void InitTimer()
    {
        isOnTimer = false;

        CurrentTimer = 0;
        rewindValue = 0;

        isRewinding = false;
        isRewindStart = false;

        UpdateVolume(true);
    }

    private void Update()
    {
        UpdateTime();
    }
    private void FixedUpdate()
    {
        StartRewindTime();
    }
    public void ChangeOnTimer(bool isOn)
    {
        isOnTimer = isOn;
    }
    public void SetRewindTime(float rewindTime)
    {
        RewindingTime = rewindTime;
        RewindManager.Instance.howManySecondsToTrack = RewindingTime;
    }
    private void UpdateVolume(bool isDefault)
    {
        defaultVolume.SetActive(isDefault);
        rewindVolume.SetActive(!isDefault);

    }
    public void UpdateText()
    {
        if (!isRewinding)
            UIManager.Instance.OnPlayTimeChange((int)CurrentTimer);
        else
            UIManager.Instance.OnRewindTimeChange((int)(RewindingTime - CurrentTimer));
    }
    private void UpdateTime()
    {
        if (!isOnTimer)                       //되감기에서 FixedUpdate로 업데이트 싸움을 해결하는 간단한 솔루션
            return;

        CurrentTimer += Time.deltaTime;

        UpdateText();

        if (CurrentTimer > RewindingTime) //시간이 다 달음
        {
            //StageTestManager.Instance.curArea.Rewind();
            //startRewind?.Invoke();
            if (isRewinding)
            {
                isRewindStart = false;
            }
            else
            {
                StageManager.Instance.CurStage.curArea.Rewind();
                UpdateVolume(false);
                isRewindStart = true;
            }

            CurrentTimer = 0;
        }
    }
    private void StartRewindTime()
    {
        if (!isOnTimer)                       //되감기에서 FixedUpdate로 업데이트 싸움을 해결하는 간단한 솔루션
            return;
        if (isRewindStart)
        {
            StartRewind();
        }
        else
        {
            if (isRewinding)
            {
                EndRewind();
            }
        }
    }
    public void StartRewind()
    {
        rewindValue += rewindIntensity;                 // 점점 더 과거로 시간을 되돌림

        if (!isRewinding)
        {
            RewindManager.Instance.StartRewindTimeBySeconds(rewindValue);
            //rewindSound.Play();
        }
        else
        {
            if (RewindManager.Instance.HowManySecondsAvailableForRewind > rewindValue)      //범위를 벗어난 값을 가져오지 않도록 안전 확인
                RewindManager.Instance.SetTimeSecondsInRewind(rewindValue);
        }
        isRewinding = true;
    }
    public void EndRewind()
    {
        Debug.Log("되감기 종료");
        RewindManager.Instance.StopRewindTimeBySeconds();
        StageManager.Instance.CurStage.curArea.ExitArea();
    }
    
}
