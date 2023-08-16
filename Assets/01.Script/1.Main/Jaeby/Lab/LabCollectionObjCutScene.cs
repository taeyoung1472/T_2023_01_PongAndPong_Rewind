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

    private void Start()
    {
        _cutSceneDirector = GetComponent<PlayableDirector>();
        AttemptCutScene();
    }

    [ContextMenu("ÇÔ¼ö : ResetPlayedData")]
    public void ResetPlayedData()
    {
        PlayerPrefs.DeleteKey("LabCollectionObjCutScene");
    }

    private void AttemptCutScene()
    {
        bool isAlreadyEnded = PlayerPrefs.GetInt("LabCollectionObjCutScene", 0) != 0;
        if (SaveDataManager.Instance.AllChapterDataBase.stageCollectionDataDic["¼­ºÎ½Ã´ë"]
            .stageCollectionValueList.Count > 0 &&
            isAlreadyEnded)
        {
            PlayerPrefs.SetInt("LabCollectionObjCutScene", 1);
            OnCutSceneStarted?.Invoke();
            _cutSceneDirector.Play();
        }
    }

    public void EndedCutScene()
    {
        Debug.Log("LabCollectionObj ÄÆ¾À ³¡");
    }
}
