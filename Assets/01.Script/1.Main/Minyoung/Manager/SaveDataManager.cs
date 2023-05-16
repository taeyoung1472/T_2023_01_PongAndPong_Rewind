using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    private ChapterStageCollectionData _chapterStageCollectionData;
    private StageCollectionData _stageCollectionData;
    private AllChapterDataBase _allChapterDataBase;

    [SerializeField] StageDatabase stageDatabase;
    private void Awake()
    {
        LoadCollectionJSON();
    }
    void LoadCollectionJSON()
    {
        string path = Application.dataPath + "/Save/ChapterCollection.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _allChapterDataBase = Newtonsoft.Json.JsonConvert.DeserializeObject<AllChapterDataBase>(json);
        }
        else
        {
            //Ŭ���� New�ϴ°�
            NewData();

            for (int i = 0; i < stageDatabase.worldList.Count; i++)
            {
                //�̸��� ���缭, �������� ������ ����
                _allChapterDataBase.stageCollectionDataDic.Add(stageDatabase.worldList[i].worldName, new ChapterStageCollectionData());
            }

            //�������� ������ŭ é�� collectionBoolDataList�� ����
            for (int i = 0; i < stageDatabase.worldList.Count; i++) //3
            {
                for (int j = 0; j < stageDatabase.worldList[i].stageList.Count; j++)
                {
                    if (_allChapterDataBase.stageCollectionDataDic[stageDatabase.worldList[i].worldName].stageCollectionDataList == null)
                    {
                        _allChapterDataBase.stageCollectionDataDic[stageDatabase.worldList[i].worldName].stageCollectionDataList = new();
                    }
                    _allChapterDataBase.stageCollectionDataDic[stageDatabase.worldList[i].worldName].stageCollectionDataList.Add(_stageCollectionData.collectionBoolDataList);
                }
            }


            for (int i = 0; i < stageDatabase.worldList.Count; i++) //3
            {
                for (int j = 0; j < _allChapterDataBase.stageCollectionDataDic[stageDatabase.worldList[i].worldName].stageCollectionDataList.Count; j++) //8 / 1/ 1
                {
                    // 8 1 1
                    Debug.Log($"{stageDatabase.worldList[i].worldName}");
                    if (_allChapterDataBase.stageCollectionDataDic[stageDatabase.worldList[i].worldName].stageCollectionDataList[j] == null)
                    {
                        Debug.Log("������ �� �ѹ����ɱ�"); //1
                        _allChapterDataBase.stageCollectionDataDic[stageDatabase.worldList[i].worldName].stageCollectionDataList[j] = new();
                    }

                     _allChapterDataBase.stageCollectionDataDic[stageDatabase.worldList[i].worldName].stageCollectionDataList[j] = stageDatabase.worldList[i].stageList[j].stageCollection;
                }
            }

            File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(_allChapterDataBase, Newtonsoft.Json.Formatting.Indented));
        }
    }
    void NewData()
    {
        if (_allChapterDataBase == null)
        {
            _allChapterDataBase = new AllChapterDataBase();
        }

        if (_allChapterDataBase.stageCollectionDataDic == null)
        {
            for (int i = 0; i < stageDatabase.worldList.Count; i++)
            {
                _allChapterDataBase.stageCollectionDataDic = new Dictionary<string, ChapterStageCollectionData>();
            }
        }

        if (_stageCollectionData == null)
        {
            _stageCollectionData = new StageCollectionData();
        }

        if (_chapterStageCollectionData == null)
        {
            _chapterStageCollectionData = new ChapterStageCollectionData();
        }

        if (_chapterStageCollectionData.stageCollectionDataList == null)
        {
            _chapterStageCollectionData.stageCollectionDataList = new();
        }
    }
}
