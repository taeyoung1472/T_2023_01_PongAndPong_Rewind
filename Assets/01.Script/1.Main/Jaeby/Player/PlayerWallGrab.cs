using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWallGrab : PlayerAction
{
    [SerializeField]
    private LayerMask _wallMask = 0;
    [SerializeField]
    private float _rayLength = 0.1f;
    [SerializeField]
    private UnityEvent<bool> OnWallGrabed = null;

    private void FixedUpdate()
    {
        WallCheck();
    }

    private void Update()
    {
        Ray ray = new Ray(_player.transform.position + _player.characterController.center, _player.PlayerRenderer.Forward);
        Debug.DrawRay(ray.origin, ray.direction * ((_player.characterController.radius) + _rayLength + _player.characterController.contactOffset), Color.red);
    }

    private void WallCheck()
    {
        if (_player.IsGrounded)
        {
            if (_excuting)
            {
                ActionExit();
                _player.PlayerAnimation.FallOrIdleAnimation(_player.IsGrounded);
            }

            return;
        }

        bool lastCheck = _excuting;
        Ray ray = new Ray(_player.transform.position + _player.characterController.center, _player.PlayerRenderer.Forward);
        _excuting = (Physics.Raycast(ray, _player.characterController.radius + _rayLength + _player.characterController.contactOffset, _wallMask)) && (_player.IsGrounded == false);
        if (lastCheck == _excuting) return;

        if (_excuting)
            WallGrabEnter();
        else
            WallGrabExit();
        OnWallGrabed?.Invoke(_excuting);
    }

    private void WallGrabExit() // ������ ����!
    {
        _player.PlayerActionLock(false, PlayerActionType.Dash, PlayerActionType.Move, PlayerActionType.WallGrab);
        _player.GravityModule.GravityScale = _player.GravityModule.OriginGravityScale;
        _player.GravityModule.UseGravity = true;
    }

    private void WallGrabEnter()
    {
        if (_locked) return;
        _player.PlayerActionExit(PlayerActionType.Dash, PlayerActionType.Jump);
        _player.VeloCityResetImm(true, true);
        _player.PlayerActionLock(true, PlayerActionType.Dash, PlayerActionType.Move, PlayerActionType.WallGrab);
        _player.GravityModule.UseGravity = true;
        _player.GravityModule.GravityScale = _player.playerMovementSO.wallSlideGravityScale;
        if (_player.playerMovementSO.wallSlideGravityScale < 0.02f)
            _player.GravityModule.UseGravity = false;
    }

    public override void ActionExit()
    {
        _excuting = false;
        WallGrabExit();
    }
}
