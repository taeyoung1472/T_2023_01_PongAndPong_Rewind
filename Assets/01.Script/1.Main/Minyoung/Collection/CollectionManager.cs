using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CollectionManager : MonoSingleTon<CollectionManager>
{
    public List<Collection> collectionObj;

    public Transform collectionParentTrm;

    [SerializeField] private StageDatabase stageDatabase;
    private void Awake()
    {
    }

    private void InitOnPlay()
    {
        SaveDataManager.Instance.LoadCollectionJSON();
    }

    private void InitOnReplay()
    {
        SetCollection();
    }

    private void Start()
    {
        collectionParentTrm = GameObject.Find("Collection").transform;
        if (collectionParentTrm == null)
        {
            return;
        }

        foreach (var item in collectionParentTrm.GetComponentsInChildren<Collection>())
        {
            collectionObj.Add(item); //add를 해주는데 add를 하기전에
        }

        if (RewindManager.Instance)
        {
            RewindManager.Instance.InitPlay += InitOnPlay;
            RewindManager.Instance.RestartPlay += InitOnReplay;
        }

        SetCollection();
    }

    public void SetCollection()
    {
        SaveDataManager.Instance.LoadCollectionJSON();
        SetStageCollecitonSO();
        SetCollectionActive();
    }
    public void SetCollectionActive()
    {
        if (StageManager.Instance.CurStageDataSO == null)
        {
            return;
        }

        for (int i = 0; i < StageManager.Instance.CurStageDataSO.stageCollection.Count; i++)
        {
            if (StageManager.Instance.CurStageDataSO.stageCollection[i] == false)
            {
                collectionObj[i].gameObject.SetActive(true);
            }
            else
            {
                collectionObj[i].gameObject.SetActive(false);
            }
        }
    }
    public void SetStageCollecitonSO()
    {
        for (int i = 0; i < stageDatabase.worldList.Count; i++) //3
        {
            for (int j = 0; j < stageDatabase.worldList[i].stageList.Count; j++) // 8 1 1
            {
                stageDatabase.worldList[i].stageList[j].stageCollection =
                    SaveDataManager.Instance.AllChapterDataBase.stageCollectionDataDic[StageManager.Instance.CurStageDataSO.chapterStageName].
                    stageCollectionDataList[j].collectionBoolDataList;
            }
        }
    }

    public void SaveClearCollection()
    {
        SaveDataManager.Instance.AllChapterDataBase.stageCollectionDataDic[StageManager.Instance.CurStageDataSO.chapterStageName].
            stageCollectionDataList[StageManager.Instance.CurStageDataSO.stageIndex].collectionBoolDataList
            = StageManager.Instance.CurStageDataSO.stageCollection;
        SaveDataManager.Instance.SaveCollectionJSON();
    }
}
