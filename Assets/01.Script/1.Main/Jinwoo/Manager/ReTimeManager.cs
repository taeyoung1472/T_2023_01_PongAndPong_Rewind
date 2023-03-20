using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReTimeManager : MonoSingleTon<ReTimeManager>
{
    // �÷��� �ð�
    private int maxPlayTime;
    private int curStagePlayTime;

    // Volume
    [Header("[Volume]")]
    [SerializeField] private GameObject defaultVolume;
    [SerializeField] private GameObject rewindVolume;

    //[SerializeField]
    //private List<ReTime> rewindObjectList = new();

    //[SerializeField] private PlayerReTime player;
    [SerializeField] private ReTime obstacle;

    public float curTime = 0;


    public int a = 0;

    public UnityEvent ReTimeStart;
    public UnityEvent ReTimeStop;
    
    private void Awake()
    {
        //player = FindObjectOfType<PlayerReTime>();
        //rewindObjectList.AddRange(FindObjectsOfType<ReTime>());
        StageManager.Instance.Init();
        Init();
    }
    public void Init()
    {


        //Debug.Log(StageManager.Instance.curArea.name);
        defaultVolume.SetActive(true);
        rewindVolume.SetActive(false);

        //ReTimeStart.AddListener(StageManager.Instance.curArea.playerPrefab.RewindStart);

        ReTimeStop?.Invoke();

        obstacle = StageManager.Instance.curArea.AreaData.reTimeObject; //�̻��� ���ľ� ���ʤ�.

        Debug.Log("��");

        
        curStagePlayTime = StageManager.Instance.curArea.PlayTime;
        curTime = 0;
    }

    private void Update()
    {
        //Debug.Log(StageManager.Instance.curArea.IsRewind);
        if (StageManager.Instance.curArea.IsRewind)
        {
            curTime -= Time.deltaTime;
            Debug.Log("������");
            //if (StageManager.Instance.curArea.IsClear)
            //{
            //    StageManager.Instance.curArea.IsClear = false;
            //    StageManager.Instance.curArea.IsRewind = false;
            //    Init();
            //}
            if (curTime <= 0 && !StageManager.Instance.curArea.IsClear) //�� �ð��� Ŭ���� x
            {
                Debug.Log("Ŭ���� ����");
                StageManager.Instance.SetArea(StageManager.Instance.curArea);
                StageManager.Instance.curArea.IsRewind = false;
                Init();
            }
            else if(curTime <= 0 && StageManager.Instance.curArea.IsClear)
            {
                Debug.Log("Ŭ������");
                //obstacle.StopTimeRewind();
            }


            return;
        }

        if (curTime >= curStagePlayTime && !StageManager.Instance.curArea.IsRewind) //���� Ÿ�̹�
        {
            //curTime = 0;
            if (!StageManager.Instance.curArea.IsRewind)
            {
                Debug.Log("?????");
                StageManager.Instance.curArea.IsRewind = true;
                PlayStartRewind();
            }
        }

        else if(!StageManager.Instance.curArea.IsRewind)
            curTime += Time.deltaTime;

        if(!StageManager.Instance.stageClear)
            UIManager.Instance.OnPlayTimeChange((int)curTime);
    }
    public void PlayStartRewind()
    {
        defaultVolume.SetActive(false);
        rewindVolume.SetActive(true);
        ReTimeStart?.Invoke();
        Debug.Log("��");
        StageManager.Instance.curArea.Rewind();
        //StageManager.Instance.curArea.player.RewindStart();
        //obstacle.StartTimeRewind();
        Debug.Log("��");
        //foreach (var re in rewindObjectList)
        //{
        //    re.StartTimeRewind();
        //}
    }

}
