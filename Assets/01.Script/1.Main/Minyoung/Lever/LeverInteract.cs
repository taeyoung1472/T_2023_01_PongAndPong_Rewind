using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverInteract : Interact
{
    [SerializeField] private ControlData[] controlDataArr;
    private Animator _animator = null;

    public bool isPush = false;

    public DirectionType gravityChangeDir;
    public override void InitOnPlay()
    {
        base.InitOnPlay();
        isPush = false;
        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.Play("Idle");
    }

    protected override void ChildInteractEnd()
    {
    }

    protected override void ChildInteractStart()
    {
        _player.PlayerInput.enabled = true;

        foreach (var control in controlDataArr)
        {
            if (control.target.isLocked)
            {
                return;
            }
        }

        LeverAnimation(isPush);
        LeverPullAction();
    }
    public void LeverPullAction()
    {
        isPush = !isPush; //처음에 ispush트루
        foreach (var control in controlDataArr)
        {
            if (isPush)
            {
                control.target.Control(control.isReverse ? ControlType.ReberseControl : ControlType.Control, true, _player, gravityChangeDir);
            }
            else
            {
                control.target.Control(control.isReverse ? ControlType.ReberseControl : ControlType.Control, false, _player, gravityChangeDir);
            }
        }
    }

    public void LeverAnimation(bool pull)
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

      

        if (pull)
        {
            _animator.Play("LeverPull");
        }
        else
        {
            _animator.Play("LeverPush");
        }
    }

}
