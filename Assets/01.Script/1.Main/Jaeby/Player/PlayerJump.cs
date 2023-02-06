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
        _player.VelocitySet(y: 0f);
        _jumpEndCheck = false;
        _excuting = true;
        _curJumpCount++;
        if (_jumpCoroutine != null)
            StopCoroutine(_jumpCoroutine);
        _jumpCoroutine = StartCoroutine(JumpCoroutine());
    }

    private IEnumerator JumpCoroutine()
    {
        float time = 1f;
        while(time > 0.5f)
        {
            _player.VelocitySet(y: _player.playerMovementSO.jumpPower * time);
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
        _player.VelocitySet(y: 0f);
    }

    public void MoreJump()
    {
        JumpEnd();
        _curJumpCount--;
        _curJumpCount = Mathf.Clamp(_curJumpCount, 0, _player.playerMovementSO.jumpCount);
    }

    public void ForceJump(Vector2 dir)
    {
        dir.Normalize();
        OnJump?.Invoke();
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
    }
}
