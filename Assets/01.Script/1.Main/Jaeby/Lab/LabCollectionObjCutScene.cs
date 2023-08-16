using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class LabCollectionObjCutScene : MonoBehaviour
{
    private PlayableDirector _cutSceneDirector = null;
    [SerializeField]
    private UnityEvent OnCutSceneStarted = null;
    [SerializeField]
    private WorldDataSO _targetWorld = null;

    private void Start()
    {
        _cutSceneDirector = transform.Find("PlayableDirector").GetComponent<PlayableDirector>();
        //AttemptCutScene();
    }

    [ContextMenu("함수 : ResetPlayedData")]
    public void ResetPlayedData()
    {
        PlayerPrefs.DeleteKey("LabCollectionObjCutScene");
    }

    [ContextMenu("가보자")]
    private void AttemptCutScene()
    {
        ResetPlayedData();
        StageCollectionData stageCollectionData = SaveDataManager.Instance.AllChapterDataBase.stageCollectionDataDic[_targetWorld.worldName]
            .stageCollectionValueList[_targetWorld.stageList[0].stageIndex];
        int currentCollection = 0;
        foreach (var e in stageCollectionData.stageDataList)
        {
            currentCollection += e.zoneCollections.collectionBoolList.FindAll(x => x == true).Count;
        }
        bool isAlreadyEnded = (PlayerPrefs.GetInt("LabCollectionObjCutScene", 0)) == 0;

        if (currentCollection > 0 && isAlreadyEnded)
        {
            Debug.Log("시작해본다");
            PlayerPrefs.SetInt("LabCollectionObjCutScene", 1);
            OnCutSceneStarted?.Invoke();
            _cutSceneDirector.Play();
        }
    }

    public void EndedCutScene()
    {
        Debug.Log("LabCollectionObj 컷씬 끝");
    }
}
