using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoSingleTon<TutorialManager>
{
    [SerializeField]
    private List<StageCommunicationSO> _dialogs = new List<StageCommunicationSO>();
    private int _index = 0;

    public bool isTutorialStage = true;

    private void Start()
    {
        isTutorialStage = true;
        //StageManager.Instance.GetCurArea().areaEndEvent.AddListener(DialogChange);
    }



    public void DialogChange()
    {
        _index++;
        if(_index < _dialogs.Count)
        {
            StageCommunicationUI.Instance.CommunicationStart(_dialogs[_index]);
        }
        //if(_index >= _dialogs.Count - 1)
        //{
        //    MainMenuManager.isPlayEventCheck = false;
        //}
    }
}
