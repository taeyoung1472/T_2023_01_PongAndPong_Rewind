using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TimerManager : MonoSingleTon<TimerManager>
{
    #region ������
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

    [SerializeField] private float rewindIntensity = 0.01f;          //�ǰ��� �ӵ��� �����ϴ� ����
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
        CurrentTimer = 0;
        rewindValue = 0;

        isOnTimer = false;
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
    public void SetRewindTime(float rewindTime)
    {
        RewindingTime = rewindTime;
        RewindTestManager.Instance.howManySecondsToTrack = RewindingTime;
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
        if (!isOnTimer)                       //�ǰ��⿡�� FixedUpdate�� ������Ʈ �ο��� �ذ��ϴ� ������ �ַ��
            return;

        CurrentTimer += Time.deltaTime;

        UpdateText();

        if (CurrentTimer > RewindingTime) //�ð��� �� ����
        {
            //StageTestManager.Instance.curArea.Rewind();
            //startRewind?.Invoke();
            if (isRewinding)
            {
                isRewindStart = false;
            }
            else
            {
                StageManager.Instance.currentStage.curArea.Rewind();
                UpdateVolume(false);
                isRewindStart = true;
            }

            CurrentTimer = 0;
        }
    }
    private void StartRewindTime()
    {
        if (!isOnTimer)                       //�ǰ��⿡�� FixedUpdate�� ������Ʈ �ο��� �ذ��ϴ� ������ �ַ��
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
        rewindValue += rewindIntensity;                 // ���� �� ���ŷ� �ð��� �ǵ���

        if (!isRewinding)
        {
            RewindTestManager.Instance.StartRewindTimeBySeconds(rewindValue);
            //rewindSound.Play();
        }
        else
        {
            if (RewindTestManager.Instance.HowManySecondsAvailableForRewind > rewindValue)      //������ ��� ���� �������� �ʵ��� ���� Ȯ��
                RewindTestManager.Instance.SetTimeSecondsInRewind(rewindValue);
        }
        isRewinding = true;
    }
    public void EndRewind()
    {
        Debug.Log("�ǰ��� ����");
        RewindTestManager.Instance.StopRewindTimeBySeconds();
        StageManager.Instance.currentStage.curArea.ExitArea();
    }
    
}
