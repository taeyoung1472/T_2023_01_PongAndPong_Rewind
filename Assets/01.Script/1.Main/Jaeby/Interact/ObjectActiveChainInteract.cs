using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActiveChainInteract : ChainInteract
{
    [SerializeField]
    private GameObject _panelObj = null;

    public override void ChildInteractEnd()
    {
        _panelObj.SetActive(false);
    }

    public override void InteractStart()
    {
        _panelObj.SetActive(true);
    }
}
