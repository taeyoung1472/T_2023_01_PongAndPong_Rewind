using System;
using UnityEngine;
using UnityEngine.Events;

public class TimerManager : MonoSingleTon<TimerManager>
{
    #region ������
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
    private Player player;

    [HideInInspector] public bool isOnTimer = false;


    private bool isRewindStart = false;
    [HideInInspector] public bool isRewinding = false;

    private float fastTimeIntensity = 1f;
    private float rewindIntensity = 0.02f;          //�ǰ��� �ӵ��� �����ϴ� ����
    private float rewindValue = 0;

    [Header("[Volume]")]
    [SerializeField] private GameObject defaultVolume;
    [SerializeField] private GameObject rewindVolume;

    public UnityEvent startRewind;
    public Action<float> OnTimeChange;

    #endregion
    private void Awake()
    {
        //InitTimer();
    }
    public void InitTimer()
    {
        isOnTimer = false;

        UIManager.Instance.ResetFastForwardTime();
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
    public void StopTime()
    {
        player = StageManager.Instance.GetCurrentPlayer().GetComponent<Player>();
        Time.timeScale = 0f;
        if (player != null)
        {
            player.PlayerInput.InputVectorReset();
            player.PlayerInput.enabled = false;
        }
    }
    public void FastForwardTimeIntensity()
    {
        JinwooVolumeManager.Instance.EnableFastTimeEffect();
        player = StageManager.Instance.GetCurrentPlayer().GetComponent<Player>();
        Time.timeScale = 3f;
        if (player != null)
        {
            player.PlayerInput.InputVectorReset();
            player.PlayerInput.enabled = false;
        }
    }
    public void ResetFastForwardTime()
    {
        JinwooVolumeManager.Instance.DisableFastTimeEffect();
        Time.timeScale = 1f;
        if (player != null)
        {
            player.PlayerInput.InputVectorReset();
            player.PlayerInput.enabled = true;
        }
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
            OnTimeChange.Invoke(CurrentTimer);
        else
            OnTimeChange.Invoke(RewindingTime - CurrentTimer);
    }
    private void UpdateTime()
    {
        if (!isOnTimer)                       //�ǰ��⿡�� FixedUpdate�� ������Ʈ �ο��� �ذ��ϴ� ������ �ַ��
            return;

        CurrentTimer += Time.deltaTime * fastTimeIntensity;

        UpdateText();
        //if (CurrentTimer > RewindingTime - 0.5f)
        //{
        //    StageManager.Instance.PlayShockWave();
        //}
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
                StageManager.Instance.CurStage.curArea.Rewind();
                UIManager.Instance.ResetFastForwardTime();
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
            RewindManager.Instance.StartRewindTimeBySeconds(rewindValue);
            //rewindSound.Play();
        }
        else
        {
            if (RewindManager.Instance.HowManySecondsAvailableForRewind > rewindValue)      //������ ��� ���� �������� �ʵ��� ���� Ȯ��
                RewindManager.Instance.SetTimeSecondsInRewind(rewindValue);
        }
        isRewinding = true;
    }
    public void EndRewind()
    {
        Debug.Log("�ǰ��� ����");
        RewindManager.Instance.StopRewindTimeBySeconds();
        StageManager.Instance.CurStage.curArea.ExitArea();
    }

}
