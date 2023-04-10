using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DigitalRuby.ThunderAndLightning;

public class TimerManager : MonoSingleTon<TimerManager>
{
    #region ������
    [SerializeField] private LightningBoltPrefabScript[] lightningBolts;
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
        if (isDefault)
        {
            //Camera.main.backgroundColor = Color.black;
            foreach (var bolt in lightningBolts)
            {
                bolt.LightningTintColor = new Color(0.5f, 0f, 0.5f);
                bolt.GlowTintColor = new Color(0.5f, 0f, 0.5f);
            }
        }
        else
        {
            //Camera.main.backgroundColor = new Color(0.5f, 0f, 0.5f);
            foreach (var bolt in lightningBolts)
            {
                bolt.LightningTintColor = Color.yellow;
                bolt.GlowTintColor = Color.yellow;
            }
        }
    }
    public void UpdateText()
    {
        if (!isRewinding)
            UIManager.Instance.OnPlayTimeChange(CurrentTimer);
        else
            UIManager.Instance.OnRewindTimeChange((RewindingTime - CurrentTimer));
    }
    private void UpdateTime()
    {
        if (!isOnTimer)                       //�ǰ��⿡�� FixedUpdate�� ������Ʈ �ο��� �ذ��ϴ� ������ �ַ��
            return;

        CurrentTimer += Time.deltaTime;

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
