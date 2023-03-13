using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ElevatorInteract : Interact
{
    [SerializeField]
    private Transform _playerPosition = null;
    private Elevator _elevator = null;

    private bool _interacting = false;
    private Vector2 targetDir = Vector2.zero;

    private void Start()
    {
        _elevator = GetComponent<Elevator>();
    }

    protected override void ChildInteractEnd()
    {
    }

    protected override void ChildInteractStart()
    {
        ElevatorManager.Instance.TargetElevatorSet(_elevator.MyIndex);

        Sequence seq = DOTween.Sequence();
        targetDir = _playerPosition.position - _player.transform.position;
        targetDir.y = 0f;
        _interacting = true;
        _player.PlayerAnimation.MoveAnimation(targetDir);
        seq.Append(_player.transform.DOMoveX(_playerPosition.position.x, 1f));
        seq.AppendCallback(() =>
        {
            _interacting = false;
            _player.PlayerAnimation.MoveAnimation(Vector2.zero);
        });
        seq.Append(_player.transform.DORotate(Vector2.zero, 0.5f));
        seq.AppendCallback(() =>
        {
            _player.transform.rotation = Quaternion.identity;
            ElevatorManager.Instance.PlayCutScene();
        });
    }

    public override void InteractEnter()
    {
        UIGetter.Instance.GetInteractUI(_interactUIPos.position, _interactSprite, KeyManager.keys[InputType.Interact]);
    }

    public override void InteractExit()
    {
        UIGetter.Instance.PushUIs();
    }

    private void Update()
    {
        if (_interacting == false)
            return;
        PlayerFlip();
    }

    private void PlayerFlip()
    {
        _player.PlayerRenderer.Flip(targetDir);
    }
}
