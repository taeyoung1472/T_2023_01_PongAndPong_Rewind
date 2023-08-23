using SCPE;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndManager : MonoSingleTon<EndManager>
{
    #region NextStage UI
    [SerializeField] private GameObject nextStagePanel;
    public GameObject NextStagePanel => nextStagePanel;

    [SerializeField] private Image nextStageExplainImage;
    [SerializeField] private Button nextCloserSpriteBtn;
    #endregion

    #region ±×³É ²¢À»¶§ UI
    [SerializeField] private GameObject endPanel;
    public GameObject EndPanel => endPanel;

    [SerializeField] private Button closerSpriteBtn;
    [SerializeField] private Button nextStageBtn;

    //[SerializeField] private TextMeshProUGUI resultText;
    //[SerializeField] private TextMeshProUGUI timeCrackText;
    [SerializeField] private TextMeshProUGUI timePieceText;
    [SerializeField] private TextMeshProUGUI currentStageNumberText;
    [SerializeField] private Image stageExplainImage;
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

        closerSpriteBtn.onClick.AddListener(() =>
        {
            isCloser = !isCloser;
            closerSpriteBtn.GetComponentInChildren<TextMeshProUGUI>().text = (isCloser) ? "¾àµµ" : "¸Ê";
            stageExplainImage.sprite = (isCloser) ? StageManager.stageDataSO.closerStageSprite : StageManager.stageDataSO.stageSprite;
        });


        nextCloserSpriteBtn.onClick.AddListener(() =>
        {
            isCloser = !isCloser;
            nextCloserSpriteBtn.GetComponentInChildren<TextMeshProUGUI>().text = (isCloser) ? "¾àµµ" : "¸Ê";
            nextStageExplainImage.sprite = (isCloser) ? StageManager.stageDataSO.nextStageData.closerStageSprite : StageManager.stageDataSO.nextStageData.stageSprite;
        });
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
        if(!isEnd)
        {
            AudioManager.PlayAudio(SoundType.OnStageClear);
        }

        isEnd = true;
        onEnd?.Invoke();
        nextStagePanel.SetActive(false);
        endPanel.SetActive(true);

        isCloser = false;
        closerSpriteBtn.GetComponentInChildren<TextMeshProUGUI>().text = "¸Ê";
        stageExplainImage.sprite = StageManager.stageDataSO.stageSprite;

        StageCollectionData stageCollectionData = SaveDataManager.Instance.AllChapterDataBase.stageCollectionDataDic
            [StageManager.Instance.CurStageDataSO.chapterStageName].stageCollectionValueList[StageManager.Instance.CurStageDataSO.stageIndex];


        int eatCnt = 0;

        foreach (var e in stageCollectionData.stageDataList)
        {
            eatCnt += e.zoneCollections.collectionBoolList.FindAll(x => x == true).Count;
        }

        timePieceText.SetText("È¹µæÇÑ ½Ã°£ÀÇ Á¶°¢" + eatCnt + "/" + StageManager.Instance.CurStageDataSO.stageCollection.Count);
        currentStageNumberText.SetText(StageManager.stageDataSO.stageNumber.ToString());

        nextStageBtn.interactable = StageManager.stageDataSO.nextStageData != null;
        nextStageBtn.onClick.AddListener(() => nextStageBtn.interactable = false);

        if(StageManager.stageDataSO.nextStageData == null)
        {
            LoadingSceneManager.LoadScene(3);
        }
    }
    public void ReStart()
    {
        blur.intensity.value = 0f;
        LoadingSceneManager.LoadScene(1);
    }
    public void NextStage()
    {
        endPanel.SetActive(false);
        nextStagePanel.SetActive(true);

        isCloser = false;
        nextCloserSpriteBtn.GetComponentInChildren<TextMeshProUGUI>().text = "¸Ê";
        nextStageExplainImage.sprite = StageManager.stageDataSO.nextStageData.stageSprite;
    }

    public void JoinStage()
    {
        blur.intensity.value = 0f;
        endPanel.SetActive(false);
        nextStagePanel.SetActive(false);
        GameManager.Instance.LoadNextStage();
    }
}
