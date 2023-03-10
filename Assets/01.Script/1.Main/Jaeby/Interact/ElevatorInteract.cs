using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ElevatorInteract : Interact
{
    [SerializeField]
    private Transform _playerPosition = null;
    [SerializeField]
    private GameObject _doInteractIcon = null;
    [SerializeField]
    private PlayableDirector _elevatorCutScene = null;

    protected override void ChildInteractEnd()
    {
        throw new System.NotImplementedException();
    }

    protected override void ChildInteractStart()
    {
        throw new System.NotImplementedException();
    }

    public override void InteractEnter()
    {
        UIGetter.Instance.GetInteractUI(_canvas, _interactUIPos.position, _interactSprite, KeyCode.F);
    }

    public override void InteractExit()
    {
        UIGetter.Instance.PushUIs();
    }
}
