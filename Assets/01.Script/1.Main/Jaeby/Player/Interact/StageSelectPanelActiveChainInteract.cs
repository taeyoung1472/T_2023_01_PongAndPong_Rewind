using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectPanelActiveChainInteract : ChainInteract
{
    [SerializeField]
    private GameObject _panelObj = null;

    public override void InteractEnd()
    {
        _player.PlayerActionExit(PlayerActionType.Interact);
    }

    public override void InteractStart()
    {
        _panelObj.SetActive(true);
    }
}
