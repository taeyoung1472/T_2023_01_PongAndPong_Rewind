using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    //[SerializeField] GameObject particles;
    //RewindTestManager rewindManager;
    public float timerDefault = 5;    
    public float CurrentTimer { get; set; }


    private bool isRewindStart = false;
    private bool isRewinding = false;
    [SerializeField] private float rewindIntensity = 0.01f;          //�ǰ��� �ӵ��� �����ϴ� ����
    //[SerializeField] RewindTestManager rewindManager;
    [SerializeField] private AudioSource rewindSound;
    private float rewindValue = 0;

    [Header("[Volume]")]
    [SerializeField] private GameObject defaultVolume;
    [SerializeField] private GameObject rewindVolume;

    private void Awake()
    {
        
        InitData();
    }
    private void Start()
    {
        
        //CurrentTimer = timerDefault;
        //rewindManager = FindObjectOfType<RewindTestManager>();
    }
    public void InitData()
    {
        CurrentTimer = 0;
        //rewindSound.Stop();
        rewindValue = 0;
        isRewinding = false;
        isRewindStart = false;
        defaultVolume.SetActive(true);
        rewindVolume.SetActive(false);
        timerDefault = StageManager.Instance.CurStage.curArea.stagePlayTime;
    }
    void Update()                               
    {
        //if (!StageTestManager.Instance.isStageAreaPlayStart)                       //�ǰ��⿡�� FixedUpdate�� ������Ʈ �ο��� �ذ��ϴ� ������ �ַ��
        //    return;

        CurrentTimer += Time.deltaTime;

        //timeText.text = "Time : " + CurrentTimer.ToString("0");
        //timeText.SetText($"{CurrentTimer}");
        if(!isRewinding)
            UIManager.Instance.OnPlayTimeChange((int)CurrentTimer);
        else
            UIManager.Instance.OnRewindTimeChange((int)(timerDefault - CurrentTimer));

        if(CurrentTimer > timerDefault + 1) //�ð��� �� ����
        {
            defaultVolume.SetActive(false);
            rewindVolume.SetActive(true);
            StageManager.Instance.CurStage.curArea.Rewind();
            if (isRewinding)
                isRewindStart = false;
            else
                isRewindStart = true;
            Debug.Log(isRewindStart);
            //particles.SetActive(!particles.activeSelf);
            CurrentTimer = 0;
        }
    }
    private void FixedUpdate()
    {
        StartRewindTime();
    }
    private void StartRewindTime()
    {
        if (isRewindStart)
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
        else
        {
            if (isRewinding)
            {
                Debug.Log("�ǰ��� ����");
                RewindTestManager.Instance.StopRewindTimeBySeconds();
                InitData();
            }
        }
    }
    public void SetText(float value)
    {
        //timeText.text = "Time : " + value.ToString("0");
        timeText.SetText($"{(int)value}");
    }
}
