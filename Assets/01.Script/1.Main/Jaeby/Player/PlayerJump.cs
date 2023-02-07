using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerJump : PlayerAction
{
    private int _curJumpCount = 0; // 현재 점프 횟수
    private bool _jumpEndCheck = false; // 점프를 한 뒤 시간이 지난 것과 바닥에 닿았을 때 둘 중 체크되면 하나가 안 되게

    [SerializeField]
    private UnityEvent OnJump = null;
    [SerializeField]
    private UnityEvent OnWallGrabJump = null;
    private Coroutine _jumpCoroutine = null;


    public void OnGrounded(bool val)
    {
        if (val == false)
            return;
        _curJumpCount = 0;
    }

    public void JumpStart()
    {
        if (_locked || _curJumpCount >= _player.playerMovementSO.jumpCount)
            return;

        _player.GravityModule.UseGravity = false;
        _jumpEndCheck = false;
        _excuting = true;
        _curJumpCount++;

        if (_jumpCoroutine != null)
            StopCoroutine(_jumpCoroutine);

        if (_player.PlayeActionCheck(PlayerActionType.WallGrab)) // 월점프!!
        {
            Debug.Log("월점프");
            _player.VeloCityResetImm(x: true, y: true);
            _player.PlayerActionExit(PlayerActionType.WallGrab);
            //_player.PlayerRenderer.ForceFlip();
            _jumpCoroutine = StartCoroutine(JumpCoroutine(Vector2.up));
        }
        else
        {
            _player.VeloCityResetImm(y: true);
            _jumpCoroutine = StartCoroutine(JumpCoroutine(Vector2.up));
        }
        OnJump?.Invoke();
    }

    private IEnumerator JumpCoroutine(Vector2 dir)
    {
        float time = 1f;
        while (time > 0f)
        {
            _player.VelocitySetExtra(dir.x * _player.playerMovementSO.jumpPower * time, dir.y * _player.playerMovementSO.jumpPower * time);
            time -= Time.deltaTime * (1f / _player.playerMovementSO.jumpHoldTime);
            yield return null;
        }
        JumpEnd();
    }

    public void JumpEnd()
    {
        if (_jumpEndCheck)
            return;

        _player.GravityModule.UseGravity = true;
        _jumpEndCheck = true;
        _excuting = false;
        if (_jumpCoroutine != null)
            StopCoroutine(_jumpCoroutine);
        _player.VelocitySetExtra(y: 0f);
    }

    public void MoreJump()
    {
        JumpEnd();
    }

    private void JumpCountUp()
    {
        _curJumpCount--;
        _curJumpCount = Mathf.Clamp(_curJumpCount, 0, _player.playerMovementSO.jumpCount);
    }

    public void TryGravityUp(Vector2 input)
    {
        if (_player.IsGrounded || input.y > -0.1f)
            return;
        _player.GravityModule.GravityScale = _player.playerMovementSO.downGravityScale;
    }

    public override void ActionExit()
    {
        _jumpEndCheck = false;
        JumpEnd();
        if (_player.PlayeActionCheck(PlayerActionType.WallGrab))
            JumpCountUp();
    }
}
