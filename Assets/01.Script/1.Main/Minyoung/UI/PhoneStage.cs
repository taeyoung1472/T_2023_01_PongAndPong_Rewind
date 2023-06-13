using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PhoneStage : MonoSingleTon<PhoneStage>
{
    private StageDataSO currentStageDataSO;

    [SerializeField] private Image stageImage;
    [SerializeField] private TextMeshProUGUI collectionText;

    [SerializeField] private Transform parentTrm;

    [SerializeField] private GameObject gimmickIcon;

    [SerializeField] private RectTransform explainUI;
    public RectTransform ExplainUI => explainUI;
    private void Start()
    {
        SetStageData();
    }

    public void SetStageData()
    {
        if (StageManager.Instance.CurStageDataSO != null)
        {
            currentStageDataSO = StageManager.Instance.CurStageDataSO;
        }
    }

    public void OnStageMenu()
    {
        if (parentTrm.childCount < currentStageDataSO.useGimmickStageList.Count)
        {
            Debug.Log("»ý¼º");
            for (int i = 0; i < currentStageDataSO.useGimmickStageList.Count; i++)
            {
                GameObject obj = Instantiate(gimmickIcon, parentTrm);
                obj.GetComponent<Image>().sprite = currentStageDataSO.useGimmickStageList[i].gimmickIcon;
                obj.GetComponent<GimmickIcon>().gimmickInfoSO = currentStageDataSO.useGimmickStageList[i];
            }
        }
    }

}
