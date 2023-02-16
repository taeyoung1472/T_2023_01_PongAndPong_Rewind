using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interact : MonoBehaviour
{
    protected bool _interactable = true;
    public bool Interactable { get => _interactable; set => _interactable = value; }

    [SerializeField]
    protected ChainInteract _chainInteract = null;

    protected Player _player = null;

    public void InteractStart(Player player)
    {
        if (_interactable == false)
            return;
        _player = player;
        ChildInteractStart();
    }
    public void InteractEnd()
    {
        ChildInteractEnd();

        if (_chainInteract == null)
            _player.PlayerActionExit(PlayerActionType.Interact);
    }

    protected abstract void ChildInteractEnd();

    protected abstract void ChildInteractStart();

    public virtual void InteractEnter()
    {

    }

    public virtual void InteractExit()
    {

    }
}
