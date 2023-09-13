using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class CollectionManager : MonoSingleTon<CollectionManager>
{
    public List<Collection> collectionObj;

    [SerializeField] private StageDatabase stageDatabase;

    private bool _isTutorialStage = false;

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
        _isTutorialStage = FindObjectOfType<TutorialManager>() != null;
        if (_isTutorialStage)
        {
            return;
        }

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
        _isTutorialStage = FindObjectOfType<TutorialManager>() != null;
        if (_isTutorialStage)
        {
            return;
        }

        for (int i = 0; i < stageDatabase.worldList.Count; i++) //i 챕터수
        {
            for (int j = 0; j < stageDatabase.worldList[i].stageList.Count; j++) //스테이지 수
            {
                ChapterStageCollectionData chapterData = SaveDataManager.Instance.AllChapterDataBase.stageCollectionDataDic
                    [StageManager.Instance.CurStageDataSO.chapterStageName];



                for (int k = 0; k < stageDatabase.worldList[i].stageList[j].stageCollection.Count; k++) //스테이지의 존 수
                {
                    stageDatabase.worldList[i].stageList[j].stageCollection[k].zone = chapterData.stageCollectionValueList[j].stageDataList[k].zoneCollections.collectionBoolList;
                }
            }
        }
    }

    public void SaveClearCollection()
    {
        //SaveDataManager.Instance.AllChapterDataBase.stageCollectionDataDic[StageManager.Instance.CurStageDataSO.chapterStageName].
        //    stageCollectionValueList[StageManager.Instance.CurStageDataSO.stageIndex].stageDataList
        //    = StageManager.Instance.CurStageDataSO.stageCollection;


        SaveDataManager.Instance.SaveCollectionJSON();
    }
}
