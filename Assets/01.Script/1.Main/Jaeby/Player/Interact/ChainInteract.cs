using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChainInteract : MonoBehaviour
{
    protected Player _player = null;
    [SerializeField]
    private ChainInteract _chainInteract = null;

    public void Init(Player player)
    {
        _player = player;
    }

    public abstract void InteractStart();

    public abstract void ChildInteractEnd();

    public void InteractEnd()
    {
        ChildInteractEnd();
        if(_chainInteract == null)
            _player.PlayerActionExit(PlayerActionType.Interact);
    }
}
