using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalNPC : MonoBehaviour
{
    [SerializeField]
    private StageSelectUI _stageSelectUI = null;
    [SerializeField]
    private List<GameObject> _stageWorldUIs = new List<GameObject>();

    public void PortalInit()
    {
        _stageSelectUI.Init(_stageWorldUIs);
    }
}
