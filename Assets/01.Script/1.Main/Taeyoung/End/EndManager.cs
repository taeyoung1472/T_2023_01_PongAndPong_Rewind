using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndManager : MonoSingleTon<EndManager>
{
    [SerializeField] private GameObject endPanel;
    [SerializeField] private Button nextStageBtn;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI timeCrackText;
    [SerializeField] private TextMeshProUGUI timePieceText;
    [SerializeField] private TextMeshProUGUI currentStageNumberText;
    [SerializeField] private Image stageExplainImage;
    private int timePiece;
    Player player;
    public void End()
    {
        endPanel.SetActive(true);

        stageExplainImage.sprite = StageManager.stageDataSO.stageSprite;
        timePieceText.SetText("È¹µæÇÑ ½Ã°£ÀÇ Á¶°¢" + timePiece + "/1");
        currentStageNumberText.SetText(StageManager.stageDataSO.stageNumber.ToString());
        nextStageBtn.interactable = StageManager.stageDataSO.nextStageData != null;
        nextStageBtn.onClick.AddListener(() => nextStageBtn.interactable = false);


    }
    public void ReStart()
    {

    }
    public void NextStage()
    {

    }
}
