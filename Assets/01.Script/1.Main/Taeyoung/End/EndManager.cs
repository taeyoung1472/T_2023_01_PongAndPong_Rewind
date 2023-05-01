using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndManager : MonoSingleTon<EndManager>
{
    [SerializeField] private GameObject endPanel;
    [SerializeField] private Button nextStageBtn;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI timeCrackText;

    public void End()
    {
        endPanel.SetActive(true);
        nextStageBtn.interactable = StageManager.stageDataSO.nextStageData != null;
        nextStageBtn.onClick.AddListener(() => nextStageBtn.interactable = false);
    }
}
