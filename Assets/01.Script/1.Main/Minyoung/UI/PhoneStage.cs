using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;

public class PhoneStage : MonoSingleTon<PhoneStage>
{
    private StageDataSO currentStageDataSO;

    [SerializeField] private Image stageImage;
    [SerializeField] private TextMeshProUGUI collectionText;

    [SerializeField] private Transform parentTrm;

    [SerializeField] private GameObject gimmickIcon;

    [SerializeField] private RectTransform explainUI;

    [SerializeField] private TextMeshProUGUI titleText;
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
            Debug.Log(currentStageDataSO);
        }
    }

    public void OnStageMenu()
    {
        int currentStageIndex = currentStageDataSO.stageIndex;
        titleText.SetText($"{currentStageIndex + 1} 스테이지");

        SaveDataManager.Instance.LoadCollectionJSON();
        StageCollectionData stageCollectionData = SaveDataManager.Instance.AllChapterDataBase.stageCollectionDataDic[currentStageDataSO.chapterStageName].stageCollectionValueList[currentStageDataSO.stageIndex];


        int eatCnt = 0;

        foreach (var e in stageCollectionData.stageDataList)
        {
            eatCnt += e.zoneCollections.collectionBoolList.FindAll(x => x == true).Count;
        }


        int totalCnt = stageCollectionData.stageDataList.Count;



        collectionText.SetText($"{eatCnt}/{totalCnt}");

        if (parentTrm.childCount < currentStageDataSO.useGimmickStageList.Count)
        {
            for (int i = 0; i < currentStageDataSO.useGimmickStageList.Count; i++)
            {
                GameObject obj = Instantiate(gimmickIcon, parentTrm);

                obj.GetComponent<Image>().sprite = currentStageDataSO.useGimmickStageList[i].gimmickIcon;

                obj.GetComponent<GimmickInfoGIF>().gimmickSO = currentStageDataSO.useGimmickStageList[i];
                obj.GetComponent<Button>().onClick.AddListener(() => obj.GetComponent<GimmickInfoGIF>().PushStageInfo());
                //obj.GetComponent<GimmickIcon>().gimmickInfoSO = currentStageDataSO.useGimmickStageList[i];
            }
        }
    }


}
