using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDataManager : MonoSingleTon<SaveDataManager>
{
    [SerializeField] StageDatabase stageDatabase;

    #region  콜렉션 관련 

    public List<StageCollectionData> stageCollectionDataList;

    private StageCollectionData _stageCollectionData;
    public StageCollectionData StageCollectionData => _stageCollectionData;


    private ChapterStageCollectionData _chapterStageCollectionData;
    public ChapterStageCollectionData ChapterStageCollectionData => _chapterStageCollectionData;


    private AllChapterDataBase _allChapterDataBase;
    public AllChapterDataBase AllChapterDataBase => _allChapterDataBase;
    #endregion

    #region 스테이지 클리어 관련

    public List<StageClearData> stageClearDataList;

    private StageClearData _stageClearData;
    public StageClearData StageClearData => _stageClearData;

    private ChapterClearData _chapterClearData;
    public ChapterClearData ChapterClearData => _chapterClearData;

    private AllChapterClearDataBase _allChapterClearDataBase;
    public AllChapterClearDataBase AllChapterClearDataBase => _allChapterClearDataBase;


    #endregion
    private void Awake()
    {
        LoadCollectionJSON();
        LoadStageClearJSON();
    }
    public void LoadCollectionJSON()
    {
        string path = Application.dataPath + "/Save/ChapterCollection.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _allChapterDataBase = Newtonsoft.Json.JsonConvert.DeserializeObject<AllChapterDataBase>(json);
        }
        else
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

            if (_chapterStageCollectionData == null)
            {
                _chapterStageCollectionData = new ChapterStageCollectionData();
            }

            if (_chapterStageCollectionData.stageCollectionDataList == null)
            {
                _chapterStageCollectionData.stageCollectionDataList = new();
            }

            for (int i = 0; i < stageDatabase.worldList.Count; i++)
            {
                for (int j = 0; j < stageDatabase.worldList[i].stageList.Count; j++)
                {
                    stageCollectionDataList.Add(new StageCollectionData());
                }
            }


            for (int i = 0; i < stageDatabase.worldList.Count; i++)
            {
                //이름에 맞춰서, 스테이지 데이터 삽입
                _allChapterDataBase.stageCollectionDataDic.Add(stageDatabase.worldList[i].worldName, new ChapterStageCollectionData());
            }

            //스테이지 갯수만큼 챕터 collectionBoolDataList를 만듬
            for (int i = 0; i < stageDatabase.worldList.Count; i++) //3
            {
                for (int j = 0; j < stageDatabase.worldList[i].stageList.Count; j++)
                {
                    if (_allChapterDataBase.stageCollectionDataDic[stageDatabase.worldList[i].worldName].stageCollectionDataList == null)
                    {
                        _allChapterDataBase.stageCollectionDataDic[stageDatabase.worldList[i].worldName].stageCollectionDataList = new();
                    }
                    _allChapterDataBase.stageCollectionDataDic[stageDatabase.worldList[i].worldName].stageCollectionDataList.
                        Add(_stageCollectionData);
                }
            }


            for (int i = 0; i < stageDatabase.worldList.Count; i++)
            {
                for (int j = 0; j < _allChapterDataBase.stageCollectionDataDic[stageDatabase.worldList[i].worldName].stageCollectionDataList.Count; j++)
                {
                    if (_allChapterDataBase.stageCollectionDataDic[stageDatabase.worldList[i].worldName].stageCollectionDataList[j] == null)
                    {
                        _allChapterDataBase.stageCollectionDataDic[stageDatabase.worldList[i].worldName].stageCollectionDataList[j] = new();
                    }

                    //_allChapterDataBase.stageCollectionDataDic[stageDatabase.worldList[i].worldName].stageCollectionDataList[j].collectionBoolDataList
                    //   = stageDatabase.worldList[i].stageList[j].stageCollection;
                }
            }

            File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(_allChapterDataBase, Newtonsoft.Json.Formatting.Indented));
        }
    }

    public void SaveCollectionJSON()
    {
        string path = Application.dataPath + "/Save/ChapterCollection.json";
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(_allChapterDataBase);

        File.WriteAllText(path, json);
    }

    public void LoadStageClearJSON()
    {
        string path = Application.dataPath + "/Save/StageClear.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _allChapterClearDataBase = Newtonsoft.Json.JsonConvert.DeserializeObject<AllChapterClearDataBase>(json);
        }
        else
        {
            if (_allChapterClearDataBase == null)
            {
                _allChapterClearDataBase = new AllChapterClearDataBase();
            }


            if (_allChapterClearDataBase.stageClearDataDic == null)
            {
                for (int i = 0; i < stageDatabase.worldList.Count; i++)
                {
                    _allChapterClearDataBase.stageClearDataDic = new Dictionary<string, ChapterClearData>();
                }
            }

            if (_chapterClearData == null)
            {
                _chapterClearData = new ChapterClearData();
            }

            if (_chapterClearData.stageClearDataList == null)
            {
                _chapterClearData.stageClearDataList = new();
            }

            //for (int i = 0; i < stageDatabase.worldList.Count; i++)
            //{
            //    for (int j = 0; j < stageDatabase.worldList[i].stageList.Count; j++)
            //    {
            //        stageClearDataList.Add(new StageClearData());
            //    }
            //}

            for (int i = 0; i < stageDatabase.worldList.Count; i++)
            {
                //이름에 맞춰서, 스테이지 클리어 데이터 삽입
                _allChapterClearDataBase.stageClearDataDic.Add(stageDatabase.worldList[i].worldName, new ChapterClearData());
            }

            for (int i = 0; i < stageDatabase.worldList.Count; i++) //3
            {
                for (int j = 0; j < stageDatabase.worldList[i].stageList.Count; j++)
                {
                    if (_allChapterClearDataBase.stageClearDataDic[stageDatabase.worldList[i].worldName].stageClearDataList == null)
                    {
                        _allChapterClearDataBase.stageClearDataDic[stageDatabase.worldList[i].worldName].stageClearDataList = new();
                    }
                    _allChapterClearDataBase.stageClearDataDic[stageDatabase.worldList[i].worldName].stageClearDataList.Add(_stageClearData);
                }
            }

            for (int i = 0; i < stageDatabase.worldList.Count; i++)
            {
                for (int j = 0; j < _allChapterClearDataBase.stageClearDataDic[stageDatabase.worldList[i].worldName].stageClearDataList.Count; j++)
                {
                    if (_allChapterClearDataBase.stageClearDataDic[stageDatabase.worldList[i].worldName].stageClearDataList[j] == null)
                    {
                        _allChapterClearDataBase.stageClearDataDic[stageDatabase.worldList[i].worldName].stageClearDataList[j] = new();
                    }

                    _allChapterClearDataBase.stageClearDataDic[stageDatabase.worldList[i].worldName].stageClearDataList[j].stageClearBoolData
                       = stageDatabase.worldList[i].stageList[j].isClear;
                }
            }


            File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(_allChapterClearDataBase));
        }
    }
    public void SaveStageClearJSON()
    {
        string path = Application.dataPath + "/Save/StageClear.json";
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(_allChapterClearDataBase);

        File.WriteAllText(path, json);
    }
}
