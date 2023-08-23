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

    [ContextMenu("ÇÔ¼ö : ResetPlayedData")]
    public void ResetPlayedData()
    {
        PlayerPrefs.DeleteKey("LabCollectionObjCutScene");
    }

    [ContextMenu("°¡º¸ÀÚ")]
    private void AttemptCutScene()
    {
        //bool isAlreadyEnded = (PlayerPrefs.GetInt("LabCollectionObjCutScene", 0)) == 0;
        Player player = Utility.SearchByClass<Player>();
        bool isAlreadyEnded = !player.playerJsonData.labCollectionCutScenePlayed;
        bool enoughCount = SaveDataManager.Instance.CurrentChapterCollectionCount(_targetWorld.worldName, _index) > 0;
        if (enoughCount && isAlreadyEnded)
        {
            player.LabCollectionCutScenePlayedSave(true);
            Debug.Log("LabCollectionObj ÄÆ¾À ½ÃÀÛ");
            //PlayerPrefs.SetInt("LabCollectionObjCutScene", 1);
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
        Debug.Log("LabCollectionObj ÄÆ¾À ³¡");
    }
}
