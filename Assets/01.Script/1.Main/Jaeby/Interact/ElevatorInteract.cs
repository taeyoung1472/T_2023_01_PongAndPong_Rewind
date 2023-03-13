using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ElevatorInteract : Interact
{
    [SerializeField]
    private Transform _playerPosition = null;
    [SerializeField]
    private PlayableDirector _elevatorCutScene = null;

    protected override void ChildInteractEnd()
    {
    }

    protected override void ChildInteractStart()
    {
        _player.characterController.Move(_player.transform.position - _playerPosition.position);
        _elevatorCutScene.Play();
    }

    public override void InteractEnter()
    {
        UIGetter.Instance.GetInteractUI(_interactUIPos.position, _interactSprite, KeyManager.keys[InputType.Interact]);
    }

    public override void InteractExit()
    {
        UIGetter.Instance.PushUIs();
    }
}
