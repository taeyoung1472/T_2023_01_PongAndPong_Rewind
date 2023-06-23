using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CollectionManager : MonoSingleTon<CollectionManager>
{
    public List<Collection> collectionObj;

    [SerializeField] private StageDatabase stageDatabase;
    private void Awake()
    {
    }

    private void InitOnPlay()
    {
        SetCollection();
    }

    private void InitOnReplay()
    {
        SetCollection();
    }

    private void Start()
    {
        collectionObj = GameObject.FindObjectsOfType<Collection>().ToList();
        collectionObj.Reverse();

        if (RewindManager.Instance)
        {
            RewindManager.Instance.InitPlay += InitOnPlay;
            RewindManager.Instance.RestartPlay += InitOnReplay;
        }

        SetCollection();

        {
            //collectionParentTrm = GameObject.Find("Collection").transform;
            //if (collectionParentTrm == null)
            //{
            //    return;
            //}
            //foreach (var item in collectionParentTrm.GetComponentsInChildren<Collection>())
            //{
            //    collectionObj.Add(item); //add를 해주는데 add를 하기전에
            //}
        }
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
            for (int j = 0; j < StageManager.Instance.CurStageDataSO.stageCollection[i].zone.Count; j++)
            {
                if (StageManager.Instance.CurStageDataSO.stageCollection[i].zone[j] == false)
                {
                    collectionObj[i].gameObject.SetActive(true);
                }
                else
                {
                    collectionObj[i].gameObject.SetActive(false);
                }
            }
        }
    }
    public void SetStageCollecitonSO()
    {
        for (int i = 0; i < stageDatabase.worldList.Count; i++)
        {
            for (int j = 0; j < stageDatabase.worldList[i].stageList.Count; j++)
            {
                for (int k = 0; i < stageDatabase.worldList[i].stageList[j].stageCollection.Count; i++)
                {
                    stageDatabase.worldList[i].stageList[j].stageCollection[k].zone =
    SaveDataManager.Instance.AllChapterDataBase.stageCollectionDataDic[StageManager.Instance.CurStageDataSO.chapterStageName].
    stageCollectionValueList[j].stageDataList[k].zoneCollections.collectionBoolList;
                }

            }
        }
    }

    public void SaveClearCollection()
    {
        //SaveDataManager.Instance.AllChapterDataBase.stageCollectionDataDic[StageManager.Instance.CurStageDataSO.chapterStageName].
        //    stageCollectionDataList[StageManager.Instance.CurStageDataSO.stageIndex].collectionBoolDataList
        //    = StageManager.Instance.CurStageDataSO.stageCollection;
        //SaveDataManager.Instance.SaveCollectionJSON();
    }
}
