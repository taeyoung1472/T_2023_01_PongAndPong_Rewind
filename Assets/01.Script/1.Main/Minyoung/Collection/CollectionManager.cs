using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CollectionManager : MonoSingleTon<CollectionManager>
{
    //public List<Collection> collections;
    public List<Collection> collectionObj;

    public Transform collectionParentTrm;

    public StageCollectionData StageCollectionData;

    public ChapterStageCollectionData ChapterStageCollectionData = new ChapterStageCollectionData();

    [SerializeField] private StageWorldListSO stageWorldList;
    private void Awake()
    {
    }
    private void Start()
    {
        collectionParentTrm = GameObject.Find("Collection").transform;

        foreach (var item in collectionParentTrm.GetComponentsInChildren<Collection>())
        {
            collectionObj.Add(item);
            //StageManager.Instance.CurStageDataSO.stageCollection.Add(item.IsEat);
            Debug.Log(item);
        }

        //처음에 아이템을 트루로해
        SetCollectionActive();
    }

    public void SetCollectionActive()
    {
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

    public void LoadCollection()
    {
        string json = File.ReadAllText(Application.dataPath + "/Save/ChapterCollection.json");

        ChapterStageCollectionData = Newtonsoft.Json.JsonConvert.DeserializeObject<ChapterStageCollectionData>(json);
        if (ChapterStageCollectionData == null)
        {
            ChapterStageCollectionData = new ChapterStageCollectionData();
        }
    }

    public void SaveCollection()
    {
        SaveStageCollecitonActive();

        string path = Application.dataPath + "/Save/ChapterCollection.json";
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(ChapterStageCollectionData);

        Debug.Log("콜렉션 " + json);
        File.WriteAllText(path, json);
    }
    public void SaveStageCollecitonActive()
    {
        ChapterStageCollectionData.stageCollectionDatas[StageManager.Instance.CurStageDataSO.stageIndex].collectionData = StageManager.Instance.CurStageDataSO.stageCollection;
    }
}
