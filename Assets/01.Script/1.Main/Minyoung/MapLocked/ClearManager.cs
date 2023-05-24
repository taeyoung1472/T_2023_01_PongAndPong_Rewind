using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearManager : MonoSingleTon<ClearManager>
{

    [SerializeField] private StageDatabase stageDatabase;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void SaveClearData()
    {
        SaveDataManager.Instance.AllChapterClearDataBase.stageClearDataDic[StageManager.Instance.CurStageDataSO.chapterStageName].
            stageClearDataList[StageManager.Instance.CurStageDataSO.stageIndex].stageClearBoolData = true;
            //= StageManager.Instance.CurStageDataSO.isClear;

        SaveDataManager.Instance.SaveStageClearJSON();
    }
}
