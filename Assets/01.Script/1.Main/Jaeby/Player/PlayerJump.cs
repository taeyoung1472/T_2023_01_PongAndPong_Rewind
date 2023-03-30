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
    private Transform _jumpEffectPos = null;
    [SerializeField]
    private UnityEvent OnJump = null;
    [SerializeField]
    private UnityEvent OnWallGrabJump = null;
    private Coroutine _jumpCoroutine = null;
    private Coroutine _moveLockCoroutine = null;

    private float _jumpInputTime = 0f;
    private bool _jumpKeyUped = false;

    private void Update()
    {
        if (_excuting)
            _jumpInputTime += Time.deltaTime;
        if (_jumpKeyUped && _jumpInputTime >= _player.playerMovementSO.jumpHoldTime * 0.5f)
        {
            _jumpKeyUped = false;
            _jumpInputTime = 0f;
            JumpEnd();
        }
    }

    public void JumpKeyUp()
    {
        _jumpKeyUped = true;
    }

    public void OnGrounded(bool val)
    {
        if (val == false)
            return;
        _jumpKeyUped = false;
        _jumpInputTime = 0f;
        _curJumpCount = 0;
        _player.GravityModule.GravityScale = _player.GravityModule.OriginGravityScale;
    }
    public void ForceJump(Vector2 dir, float jumpPower)
    {
        _jumpEndCheck = false;
        _excuting = true;
        if (_jumpCoroutine != null)
            StopCoroutine(_jumpCoroutine);
        if (_moveLockCoroutine != null)
        {
            StopCoroutine(_moveLockCoroutine);
            _player.PlayerActionLock(false, PlayerActionType.Move);
        }
        _player.VeloCityResetImm(y: true);
        _jumpCoroutine = StartCoroutine(JumpCoroutine(dir, jumpPower));
        OnJump?.Invoke();
    }

    public void JumpStart()
    {
        if (_locked || _curJumpCount >= _player.playerMovementSO.jumpCount)
            return;

        _jumpEndCheck = false;
        _excuting = true;
        _curJumpCount++;
        _jumpKeyUped = false;
        _jumpInputTime = 0f;

        if (_jumpCoroutine != null)
            StopCoroutine(_jumpCoroutine);
        if (_moveLockCoroutine != null)
        {
            StopCoroutine(_moveLockCoroutine);
            _player.PlayerActionLock(false, PlayerActionType.Move);
        }

        OnJump?.Invoke();
        if (_player.PlayeActionCheck(PlayerActionType.WallGrab)) // 월점프!!
        {
            _player.PlayerActionExit(PlayerActionType.WallGrab);
            _player.VeloCityResetImm(x: true, y: true);
            _player.PlayerRenderer.ForceFlip();
            _jumpCoroutine = StartCoroutine(JumpCoroutine(_player.playerMovementSO.wallJumpPower * _player.PlayerRenderer.Forward.x, _player.playerMovementSO.wallGrabJumpPower));
            _moveLockCoroutine = StartCoroutine(MoveLockCoroutine());
        }
        else
        {
            _player.VeloCityResetImm(y: true);
            _jumpCoroutine = StartCoroutine(JumpCoroutine(transform.up, _player.playerMovementSO.jumpPower));
        }
    }

    private IEnumerator MoveLockCoroutine()
    {
        _player.PlayerActionLock(true, PlayerActionType.Move);
        yield return new WaitForSeconds(_player.playerMovementSO.moveLockTime);
        _player.PlayerActionLock(false, PlayerActionType.Move);
    }

    private IEnumerator JumpCoroutine(Vector2 dir, float jumpPower)
    {
        float time = 1f;
        while (time > 0f)
        {
            //_player.GravityModule.GravityScale = _player.GravityModule.OriginGravityScale;
            //sqrt(1 - Math.pow(x - 1, 2));
            Vector2 final = new Vector2(Mathf.Sqrt(1 - (float)Math.Pow(time - 1, 2)) * dir.x * jumpPower, Mathf.Sqrt(1 - (float)Math.Pow(time - 1, 2)) * dir.y * jumpPower);
            //Vector2 final = Vector2.up *  _player.GravityModule.GetGravity().y * (_fallMultiplier - 1) * Time.deltaTime;
            //Vector2 final = new Vector2(dir.x * jumpPower * (time * Mathf.PI) * 0.5f, dir.y * jumpPower * (time * Mathf.PI) * 0.5f);
            _player.VelocitySetExtra(final.x, final.y);
            time -= Time.deltaTime * (1f / _player.playerMovementSO.jumpHoldTime);
            yield return null;
        }
        JumpEnd();
    }

    public void JumpEnd()
    {
        if (_jumpEndCheck)
            return;
        _jumpEndCheck = true;
        _excuting = false;
        if (_jumpCoroutine != null)
            StopCoroutine(_jumpCoroutine);
        if (_moveLockCoroutine != null)
        {
            StopCoroutine(_moveLockCoroutine);
            _player.PlayerActionLock(false, PlayerActionType.Move);
        }
        _player.VelocitySetExtra(0f, 0f);
        if (_player.IsGrounded)
            OnGrounded(true);
    }

    public void MoreJump()
    {
        JumpEnd();
    }

    public void JumpCountUp()
    {
        _curJumpCount--;
        _curJumpCount = Mathf.Clamp(_curJumpCount, 0, _player.playerMovementSO.jumpCount);
    }

    public void TryGravityUp(Vector2 input)
    {
        if (_player.IsGrounded || input.y > -0.1f || _player.PlayeActionCheck(PlayerActionType.WallGrab))
            return;
        _player.GravityModule.GravityScale = _player.playerMovementSO.downGravityScale;
    }

    public override void ActionExit()
    {
        _jumpEndCheck = false;
        _jumpInputTime = 0f;
        _jumpKeyUped = false;
        JumpEnd();
        if (_player.PlayeActionCheck(PlayerActionType.WallGrab))
            JumpCountUp();
    }

    public void SpawnJumpEffect()
    {
        //flip이면 x -90

        Vector3 pos = _jumpEffectPos.position;
        Quaternion rot = Quaternion.identity;
        Transform trm = PoolManager.Pop(PoolType.JumpEffect).transform;
        if (_player.PlayeActionCheck(PlayerActionType.WallGrab))
        {
            rot = _player.PlayerRenderer.GetFlipedRotation(DirType.Forward, RotAxis.Z);
            pos += transform.up * 0.5f;
        }
        if (_player.PlayerRenderer.flipDirection == FlipDirection.Up)
        {
            rot = Quaternion.Euler(180f, 0f, 0f);
        }
        trm.SetPositionAndRotation(pos, rot);
    }
}
