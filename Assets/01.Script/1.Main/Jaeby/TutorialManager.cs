using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private List<StageCommunicationSO> _dialogs = new List<StageCommunicationSO>();
    private int _index = 0;

    private void Start()
    {
        StageManager.Instance.GetCurArea().areaEndEvent.AddListener(DialogChange);
    }

    private void DialogChange()
    {
        _index++;
        if(_index < _dialogs.Count)
        {
            StageCommunicationUI.Instance.CommunicationStart(_dialogs[_index]);
        }
    }
}
