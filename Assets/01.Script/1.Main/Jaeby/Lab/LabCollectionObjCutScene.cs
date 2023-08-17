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
    [SerializeField]
    private int _index = 1;
    [SerializeField]
    private LabCollectionObjController _labCollectionObjController = null;

    private void Start()
    {
        _cutSceneDirector = transform.Find("PlayableDirector").GetComponent<PlayableDirector>();
        AttemptCutScene();
    }

    [ContextMenu("�Լ� : ResetPlayedData")]
    public void ResetPlayedData()
    {
        PlayerPrefs.DeleteKey("LabCollectionObjCutScene");
    }

    [ContextMenu("������")]
    private void AttemptCutScene()
    {
        bool isAlreadyEnded = (PlayerPrefs.GetInt("LabCollectionObjCutScene", 0)) == 0;
        if (SaveDataManager.Instance.CurrentStageCollectionCount(_targetWorld.worldName, _index) > 0 && isAlreadyEnded)
        {
            Debug.Log("LabCollectionObj �ƾ� ����");
            PlayerPrefs.SetInt("LabCollectionObjCutScene", 1);
            OnCutSceneStarted?.Invoke();
            _cutSceneDirector.Play();
        }
        else
        {
            if(_labCollectionObjController != null)
            {
                _labCollectionObjController.PercentSet();
            }
        }
    }

    public void EndedCutScene()
    {
        Debug.Log("LabCollectionObj �ƾ� ��");
    }
}
