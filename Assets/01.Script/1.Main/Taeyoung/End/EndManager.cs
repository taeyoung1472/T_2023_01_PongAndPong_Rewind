using SCPE;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class EndManager : MonoSingleTon<EndManager>
{

    #region 그냥 깻을때 UI
    [SerializeField] private GameObject endPanel;
    public GameObject EndPanel => endPanel;

    [SerializeField] private TextMeshProUGUI timePieceText;
    [SerializeField] private TextMeshProUGUI currentStageNumberText;
    #endregion

    [SerializeField] private bool isCloser = false;
    [SerializeField] private UnityEvent onEnd;
    [SerializeField] private GameObject EndingUI;

    public Volume volume;
    private LimitlessGlitch2 glitch2;
    private DoubleVision blur;
    public bool IsEnd { get { return isEnd; } }
    private bool isEnd;


    private void Start()
    {
        volume.profile.TryGet(out glitch2);
        volume.profile.TryGet(out blur);

        glitch2.enable.value = false;
        blur.intensity.value = 0f;
    }

    public void EndVolume()
    {
        blur.intensity.value = 0f;
        glitch2.enable.value = true;
        StartCoroutine(BlurEvent());
    }
    IEnumerator BlurEvent()
    {
        yield return new WaitForSeconds(0.2f);
        while (blur.intensity.value < 1f)
        {
            if (blur.intensity.value > 0.6f)
            {
                break;
            }
            blur.intensity.value += 0.01f;
            yield return new WaitForSeconds(0.05f);
        }
        glitch2.enable.value = false;
    }
    public void End()
    {
        if (!isEnd)
        {
            AudioManager.PlayAudio(SoundType.OnStageClear);
        }

        isEnd = true;
        onEnd?.Invoke();
        endPanel.SetActive(true);

        isCloser = false;

        StageCollectionData stageCollectionData = SaveDataManager.Instance.AllChapterDataBase.stageCollectionDataDic
            [StageManager.Instance.CurStageDataSO.chapterStageName].stageCollectionValueList[StageManager.Instance.CurStageDataSO.stageIndex];


        int eatCnt = 0;

        foreach (var e in stageCollectionData.stageDataList)
        {
            eatCnt += e.zoneCollections.collectionBoolList.FindAll(x => x == true).Count;
        }

        timePieceText.SetText("획득한 시간의 조각" + eatCnt + "/" + StageManager.Instance.CurStageDataSO.stageCollection.Count);
        currentStageNumberText.SetText("스테이지 " +  StageManager.stageDataSO.stageNumber.ToString());

        if (StageManager.stageDataSO.isFirst)
        {
            LoadingSceneManager.LoadScene(0);
            return;
        }
        if (StageManager.stageDataSO.nextStageData == null)
        {
            LoadingSceneManager.LoadScene(3);
        }
    }
    public void ReStart()
    {
        blur.intensity.value = 0f;
        LoadingSceneManager.LoadScene(1);
    }

    public void JoinStage()
    {
        blur.intensity.value = 0f;
        endPanel.SetActive(false);
        GameManager.Instance.LoadNextStage();
    }
}
