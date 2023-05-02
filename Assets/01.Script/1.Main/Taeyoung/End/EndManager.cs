using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndManager : MonoSingleTon<EndManager>
{
    #region NextStage UI
    [SerializeField] private GameObject nextStagePanel;

    [SerializeField] private Image nextStageExplainImage;
    [SerializeField] private Button nextCloserSpriteBtn;
    #endregion

    #region ±×³É ²¢À»¶§ UI
    [SerializeField] private GameObject endPanel;

    [SerializeField] private Button closerSpriteBtn;
    [SerializeField] private Button nextStageBtn;

    //[SerializeField] private TextMeshProUGUI resultText;
    //[SerializeField] private TextMeshProUGUI timeCrackText;
    [SerializeField] private TextMeshProUGUI timePieceText;
    [SerializeField] private TextMeshProUGUI currentStageNumberText;
    [SerializeField] private Image stageExplainImage;
    #endregion

    private bool isCloser = false;

    public void End()
    {
        nextStagePanel.SetActive(false);
        endPanel.SetActive(true);

        isCloser = false;

        stageExplainImage.sprite = StageManager.stageDataSO.stageSprite;

        timePieceText.SetText("È¹µæÇÑ ½Ã°£ÀÇ Á¶°¢" + "1/1");
        currentStageNumberText.SetText(StageManager.stageDataSO.stageNumber.ToString());

        nextStageBtn.interactable = StageManager.stageDataSO.nextStageData != null;
        nextStageBtn.onClick.AddListener(() => nextStageBtn.interactable = false);

        closerSpriteBtn.onClick.AddListener(() =>
        {
            isCloser = !isCloser;
            closerSpriteBtn.GetComponentInChildren<TextMeshProUGUI>().text = (isCloser) ? "¾àµµ" : "¸Ê";
            stageExplainImage.sprite = (isCloser) ? StageManager.stageDataSO.closerStageSprite : StageManager.stageDataSO.stageSprite;
        });
    }
    public void ReStart()
    {
        SceneManager.LoadScene(1);
    }
    public void NextStage()
    {
        endPanel.SetActive(false);
        nextStagePanel.SetActive(true);

        isCloser = false;
        nextStageExplainImage.sprite = StageManager.stageDataSO.nextStageData.stageSprite;

        nextCloserSpriteBtn.onClick.AddListener(() =>
        {
            isCloser = !isCloser;
            nextCloserSpriteBtn.GetComponentInChildren<TextMeshProUGUI>().text = (isCloser) ? "¾àµµ" : "¸Ê";
            nextStageExplainImage.sprite = (isCloser) ? StageManager.stageDataSO.nextStageData.closerStageSprite : StageManager.stageDataSO.nextStageData.stageSprite;
        });
    }

    public void JoinStage()
    {
        endPanel.SetActive(false);
        nextStagePanel.SetActive(false);
        GameManager.Instance.LoadNextStage();
    }
}
