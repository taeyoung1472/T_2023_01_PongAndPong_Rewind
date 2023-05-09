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

    private StageCollectionData _stageCollectionData;

    private ChapterStageCollectionData _chapterStageCollectionData;

    public WorldDataSO worldDataSO;

    private void Awake()
    {

        if (RewindManager.Instance)
        {
            RewindManager.Instance.InitPlay += InitOnPlay;
        }
    }

    private void InitOnPlay()
    {
        LoadCollectionJson();
    }

    private void Start()
    {
        collectionParentTrm = GameObject.Find("Collection").transform;
        if (collectionParentTrm == null)
        {
            return;
        }
        Debug.Log("�ݷ���Ʈ����������");
        foreach (var item in collectionParentTrm.GetComponentsInChildren<Collection>())
        {
            collectionObj.Add(item);
            Debug.Log(item);
        }
        LoadCollectionJson();
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
        for (int i = 0; i < _chapterStageCollectionData.stageCollectionDatas.Count; i++)
        {
            worldDataSO.stageList[i].stageCollection = _chapterStageCollectionData.stageCollectionDatas[i].collectionData;
        }
    }

    public void LoadCollectionJson()
    {
        string path = Application.dataPath + "/Save/ChapterCollection.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _chapterStageCollectionData = Newtonsoft.Json.JsonConvert.DeserializeObject<ChapterStageCollectionData>(json);

            SetStageCollecitonSO();
            SetCollectionActive();
        }
        else
        {
            Debug.Log("�ݷ������̽������ͻ���");

            if (_chapterStageCollectionData == null)
            {
                _chapterStageCollectionData = new ChapterStageCollectionData();
                Debug.Log("���̴ϱ� ���θ��� é�͸�" + _chapterStageCollectionData);
            }


            if (_chapterStageCollectionData.stageCollectionDatas == null)
            {
                _chapterStageCollectionData.stageCollectionDatas = new List<StageCollectionData>();

                if (_stageCollectionData == null)
                {
                    _stageCollectionData = new StageCollectionData();
                    Debug.Log("���̴ϱ� ���θ��� ����������" + _stageCollectionData);
                }

                for (int i = 0; i < worldDataSO.stageList.Count; i++)
                {
                    _chapterStageCollectionData.stageCollectionDatas.Add(_stageCollectionData);
                    _chapterStageCollectionData.stageCollectionDatas[i].collectionData = worldDataSO.stageList[i].stageCollection;
                }
                Debug.Log("ó�� JSON ������ �����Ҷ��� ��� false");

                File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(_chapterStageCollectionData));
            }
        }
    }

    public void SaveCollection()
    {
        SaveStageCollecitonActive();

        string path = Application.dataPath + "/Save/ChapterCollection.json";
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(_chapterStageCollectionData);

        File.WriteAllText(path, json);
    }
    public void SaveStageCollecitonActive()
    {
        _chapterStageCollectionData.stageCollectionDatas[StageManager.Instance.CurStageDataSO.stageIndex].collectionData = StageManager.Instance.CurStageDataSO.stageCollection;
    }
    public void InitSaveJSON()
    {
        LoadCollectionJson();
        for (int i = 0; i < worldDataSO.stageList.Count - 1; i++)
        {
            for (int j = 0; j < _chapterStageCollectionData.stageCollectionDatas[i].collectionData.Count; j++)
            {
                _chapterStageCollectionData.stageCollectionDatas[i].collectionData[j] = false;
            }
        }

        string path = Application.dataPath + "/Save/ChapterCollection.json";
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(_chapterStageCollectionData);
        File.WriteAllText(path, json);
    }

    private void OnApplicationQuit()
    {
        Debug.Log("�̰Խ����");
        InitSaveJSON();        
    }

}
