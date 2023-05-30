using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhoneCollection : MonoSingleTon<PhoneCollection>
{
    private List<ChapterStageCollectionData> chapterList = new List<ChapterStageCollectionData>();
    public List<GameObject> childObjs;
    public List<string> chapterNameList = new List<string>();

    [SerializeField] private StageDatabase stageDatabase;
    [SerializeField] private TextMeshProUGUI mainText;

    [SerializeField] private Transform parentTrm;
    [SerializeField] private GameObject chapterCellPrefab;

    [SerializeField] private Transform chapterParentTrm;
    [SerializeField] private GameObject stageCellPrefab;


    void Start()
    {
        for (int i = 0; i < stageDatabase.worldList.Count; i++)
        {
            GameObject obj = Instantiate(chapterCellPrefab, parentTrm);
        }
    }
    public void OnCollectionMenu()
    {
        if (childObjs.Count < stageDatabase.worldList.Count)
        {
            foreach (Transform trm in parentTrm)
                childObjs.Add(trm.gameObject);
        }


        SaveDataManager.Instance.LoadCollectionJSON();

        foreach (var cn in SaveDataManager.Instance.AllChapterDataBase.stageCollectionDataDic)
        {
            chapterNameList.Add(cn.Key);
            chapterList.Add(SaveDataManager.Instance.AllChapterDataBase.stageCollectionDataDic[cn.Key]);
        }
        int maxCnt = 0;
        int eatCnt = 0;

        for (int i = 0; i < parentTrm.childCount; i++)
        {
            childObjs[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText($"{i + 1} √©≈Õ{chapterNameList[i]}");

            foreach (var s in chapterList[i].stageCollectionDataList)
                maxCnt += s.collectionBoolDataList.Count;

            foreach (var e in chapterList[i].stageCollectionDataList)
                eatCnt += e.collectionBoolDataList.FindAll(x => x == true).Count;

            childObjs[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText($"{eatCnt} / {maxCnt}");

            maxCnt = 0;
            eatCnt = 0;
        }
    }

    public void OnChapterCollection(int index)
    {
        mainText.SetText($"{index + 1} √©≈Õ ¡§∫∏");
        for (int i = 0; i < chapterList[index].stageCollectionDataList.Count; i++)
        {
            GameObject obj = Instantiate(stageCellPrefab, chapterParentTrm);
        }
    }


}
