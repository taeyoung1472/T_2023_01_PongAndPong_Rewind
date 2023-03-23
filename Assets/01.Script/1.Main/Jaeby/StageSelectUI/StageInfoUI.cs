using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoUI : MonoBehaviour
{
    [SerializeField]
    private StageSelectUI _stageSelectUI = null;
    [SerializeField]
    private TextMeshProUGUI _stageNameText = null;
    [SerializeField]
    private TextMeshProUGUI _stageSubTitleText = null;
    [SerializeField]
    private TextMeshProUGUI _stageExplainText = null;
    [SerializeField]
    private Image _stageImage = null;

    public void UISet(StageUnitUI ui)
    {
        _stageSelectUI.Lock = true;
        if (ui.StageDataSO == null)
            return;
        _stageNameText.SetText(ui.StageDataSO.stageName);
        _stageSubTitleText.SetText(ui.StageDataSO.stageSubTitle);
        _stageExplainText.SetText(ui.StageDataSO.stageExplain);
        _stageImage.sprite = ui.StageDataSO.stageSprite;
    }

    public void UIDown()
    {
        _stageSelectUI.Lock = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UIDown();
        }
    }
}
