using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class SaveDataManager : MonoSingleTon<SaveDataManager>
{
    private CurrentStageNameData _currentStageNameData;
    public CurrentStageNameData CurrentStageNameData => _currentStageNameData;

    [SerializeField] StageDatabase stageDatabase;

    #region  �ݷ��� ���� 
    public List<ZoneCollection> zoneCollectionsList;
    //  public List<StageCollectionData> stageCollectionDataList;

    private StageCollectionData _stageCollectionData;
    public StageCollectionData StageCollectionData => _stageCollectionData;


    private ChapterStageCollectionData _chapterStageCollectionData;
    public ChapterStageCollectionData ChapterStageCollectionData => _chapterStageCollectionData;


    private AllChapterDataBase _allChapterDataBase;
    public AllChapterDataBase AllChapterDataBase => _allChapterDataBase;


    private ZoneCollection _zoneCollection;
    public ZoneCollection ZoneCollection => _zoneCollection;

    private AllZoneData _allZoneData;
    public AllZoneData AllZoneData => _allZoneData;

    #endregion

    #region �������� Ŭ���� ����

    public List<StageClearData> stageClearDataList;

    private StageClearData _stageClearData;
    public StageClearData StageClearData => _stageClearData;

    private ChapterClearData _chapterClearData;
    public ChapterClearData ChapterClearData => _chapterClearData;

    private AllChapterClearDataBase _allChapterClearDataBase;
    public AllChapterClearDataBase AllChapterClearDataBase => _allChapterClearDataBase;


    #endregion

    private SettingValue _settingValue;
    public SettingValue SettingValue => _settingValue;
    private void Awake()
    {
        LoadCollectionJSON();
        LoadStageClearJSON();
        LoadSoundJSON();
        LoadStageInfoJSON();
    }

    public void SetStageInfo(string name, int cnt)
    {
        _currentStageNameData.worldName = name;
        _currentStageNameData.currentStageIndex = cnt;
    }
    public void SetChapterChildCount(int cnt)
    {
        _currentStageNameData.stageCnt = cnt;
    }

    public bool IsThisChapterClear(string worldName)
    {
        int cnt = 0;
        List<StageClearData> chapterClearDatas = _allChapterClearDataBase.stageClearDataDic[worldName].stageClearDataList;
        foreach (var item in chapterClearDatas)
        {
            if (item.stageClearBoolData == true)
            {
                cnt++;
            }
        }

        if (cnt == _currentStageNameData.stageCnt)
        {
            return true;

        }
        return false;


    }
    public bool CheckPlayCutScene(string worldName)
    {
        if (_currentStageNameData.cutSceneDic[worldName] == true)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public void LoadStageInfoJSON()
    {
        string path = Application.dataPath + "/Save/StageInfo.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _currentStageNameData = Newtonsoft.Json.JsonConvert.DeserializeObject<CurrentStageNameData>(json);
        }
        else
        {
            if (_currentStageNameData == null)
            {
                _currentStageNameData = new();
            }

            if (_currentStageNameData.cutSceneDic == null)
            {
                _currentStageNameData.cutSceneDic = new();
            }

            for (int i = 0; i < stageDatabase.worldList.Count; i++)
            {
                _currentStageNameData.cutSceneDic.Add(stageDatabase.worldList[i].worldName, false);
            }



            File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(_currentStageNameData));
        }
    }
    public void SaveStageInfoJSON()
    {
        string path = Application.dataPath + "/Save/StageInfo.json";
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(_currentStageNameData);

        File.WriteAllText(path, json);
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

            #region JSON ���� ��ųʸ� ���� �� ������ ����
            if (_allChapterDataBase.stageCollectionDataDic == null)
            {
                for (int i = 0; i < stageDatabase.worldList.Count; i++)
                {
                    _allChapterDataBase.stageCollectionDataDic = new Dictionary<string, ChapterStageCollectionData>();
                }
            }

            for (int i = 0; i < stageDatabase.worldList.Count; i++)
            {
                //�̸��� ���缭, �������� ������ ����
                _allChapterDataBase.stageCollectionDataDic.Add(stageDatabase.worldList[i].worldName, new ChapterStageCollectionData());
            }
            #endregion

            for (int i = 0; i < stageDatabase.worldList.Count; i++) //i é�ͼ�
            {
                for (int j = 0; j < stageDatabase.worldList[i].stageList.Count; j++) //�������� ��
                {
                    ChapterStageCollectionData chapterData = _allChapterDataBase.stageCollectionDataDic[stageDatabase.worldList[i].worldName];

                    #region JSON stageDataList ��üũ �� ����
                    if (chapterData.stageCollectionValueList == null)
                    {
                        chapterData.stageCollectionValueList = new List<StageCollectionData>();
                    }

                    chapterData.stageCollectionValueList.Add(new StageCollectionData());
                    #endregion

                    for (int k = 0; k < stageDatabase.worldList[i].stageList[j].stageCollection.Count; k++) //���������� �� ��
                    {
                        #region JSON zoneCollections ��üũ �� ����
                        if (chapterData.stageCollectionValueList[j].stageDataList == null)
                        {
                            chapterData.stageCollectionValueList[j].stageDataList = new List<AllZoneData>();
                        }
                        chapterData.stageCollectionValueList[j].stageDataList.Add(new AllZoneData());
                        #endregion

                        #region JSON collectionBoolList ����
                        if (chapterData.stageCollectionValueList[j].stageDataList[k].zoneCollections == null)
                        {
                            chapterData.stageCollectionValueList[j].stageDataList[k].zoneCollections = new ZoneCollection();
                        }
                        #endregion


                        #region JSON �ҹ迭 ��üũ �� ����
                        if (chapterData.stageCollectionValueList[j].stageDataList[k].zoneCollections.collectionBoolList == null)
                        {
                            chapterData.stageCollectionValueList[j].stageDataList[k].zoneCollections.collectionBoolList = new List<bool>();
                        }

                        chapterData.stageCollectionValueList[j].stageDataList[k].zoneCollections.collectionBoolList
                            = stageDatabase.worldList[i].stageList[j].stageCollection[k].zone;
                        #endregion
                    }
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

            if (true)
            {

            }

            for (int i = 0; i < stageDatabase.worldList.Count; i++)
            {
                //�̸��� ���缭, �������� Ŭ���� ������ ����
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
                if (stageDatabase.worldList[i].stageList == null)
                {
                    return;
                }

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
    public int CurrentStageCollectionCount(string worldName, int stageIndex)
    {
        if(stageIndex == 0)
        {
            return 0;
        }

        int cnt = 0;

        StageCollectionData stageCollectionData = _allChapterDataBase.stageCollectionDataDic[worldName]
        .stageCollectionValueList[stageIndex];


        if (stageCollectionData == null)
        {
            Debug.Log("������������");
            return 0;
        }


        foreach (var e in stageCollectionData.stageDataList)
        {
            cnt += e.zoneCollections.collectionBoolList.FindAll(x => x == true).Count;
        }
        return cnt;
    }
    public int MaxStageCollectionCount(string worldName, int stageIndex)
    {
        int cnt = 0;

     
        StageCollectionData stageCollectionData = _allChapterDataBase.stageCollectionDataDic[worldName]
        .stageCollectionValueList[stageIndex];

        if (stageCollectionData.stageDataList == null)
        {
            Debug.Log("������������ǰ����");

            return 0;
        }



        cnt = stageCollectionData.stageDataList.Count;



        return cnt;
    }


    public int CurrentChapterCollectionCount(string worldName, int index)
    {
        int cnt = 0;

        for (int i = 0; i < index; i++)
        {
            StageCollectionData stageCollectionData = _allChapterDataBase.stageCollectionDataDic[worldName]
           .stageCollectionValueList[i];

            if (stageCollectionData == null)
            {
                Debug.Log("������������");
                return 0;
            }

            foreach (var e in stageCollectionData.stageDataList)
            {
                cnt += e.zoneCollections.collectionBoolList.FindAll(x => x == true).Count;
            }

        }


        return cnt;
    }

    public bool IsStageClearPortal(string worldName, int index)
    {
        int currentCnt = CurrentStageCollectionCount(worldName, index);
        Debug.Log(currentCnt);

        int maxCnt = MaxStageCollectionCount(worldName, index);
        Debug.Log(maxCnt);

        if (currentCnt == maxCnt)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public int MaxChapterCollectionCount(string worldName, int index)
    {
        int cnt = 0;

        for (int i = 0; i < index; i++)
        {
            StageCollectionData stageCollectionData = _allChapterDataBase.stageCollectionDataDic[worldName]
           .stageCollectionValueList[i];

            if (stageCollectionData.stageDataList == null)
            {
                Debug.Log("������������ǰ����");

                return 0;
            }


            cnt += stageCollectionData.stageDataList.Count;
        }
        return cnt;
    }

    /// <summary>
    /// é�ʹ���
    /// </summary>
    /// <param name="worldName"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool IsChapterClearPortal(string worldName, int index)
    {
        int currentCnt = CurrentChapterCollectionCount(worldName, index);
        Debug.Log("���� " + currentCnt);
        int maxCnt = MaxChapterCollectionCount(worldName, index);
        Debug.Log("�ƽ�" + maxCnt);


        if (currentCnt == maxCnt)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void LoadSoundJSON()
    {
        string path = Application.dataPath + "/Save/SettingValue.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _settingValue = Newtonsoft.Json.JsonConvert.DeserializeObject<SettingValue>(json);
        }
        else
        {
            if (_settingValue == null)
            {
                _settingValue = new();
            }
            File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(_settingValue));
        }
    }

    public void SaveSoundJSON()
    {
        string path = Application.dataPath + "/Save/SettingValue.json";
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(_settingValue);

        File.WriteAllText(path, json);
    }

    public void SettingJSON(bool isMute, float sound, bool isScreen, int index)
    {
        _settingValue.isMute = isMute;
        _settingValue.volume = sound;
        _settingValue.isFullScreen = isScreen;
        _settingValue.fpsLimitIndex = index;
    }

}
