using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDash : PlayerAction
{
    [SerializeField]
    private UnityEvent<Vector2> OnDashStarted = null;
    [SerializeField]
    private UnityEvent<bool> OnDashEnded = null;

    private int _curDashCount = 0;
    private Coroutine _dashCoroutine = null; // 대시
    private Coroutine _dashChargeCoroutine = null; // 땅에 닿아있을 때 일정 시간 뒤에 대시 가능하게

    public void Dash()
    {
        if (_locked || _excuting || _curDashCount >= _player.playerMovementSO.dashCount ||
            _player.PlayerInput.InputVectorNorm.sqrMagnitude == 0f ||
            (Mathf.Abs(_player.PlayerInput.InputVector.x) > 0f == false) ||
            _player.PlayerActionCheck(PlayerActionType.ObjectPush))
            return;
        _excuting = true;

        bool slide = _player.IsGrounded;

        if (_dashChargeCoroutine != null)
        {
            StopCoroutine(_dashChargeCoroutine);
        }
        if (_dashCoroutine != null)
        {
            StopCoroutine(_dashCoroutine);
            DashExit();
        }

        _dashCoroutine = StartCoroutine(DashCoroutine(slide));

        GameObject effectObj = PoolManager.Pop(PoolType.DashEffect);
        effectObj.transform.SetPositionAndRotation(transform.position + transform.up * 0.65f + transform.forward * 0.15f, _player.PlayerRenderer.GetFlipedRotation(DirType.Back, RotAxis.Y));
        OnDashStarted?.Invoke(_player.PlayerInput.InputVectorNorm);
    }

    public void MoreDash(int cnt)
    {
        ActionExit();
        _curDashCount = cnt;
        if (_curDashCount < 0)
            _curDashCount = 0;
    }

    private IEnumerator DashCoroutine(bool slide)
    {
        _player.PlayerActionExit(PlayerActionType.Jump, PlayerActionType.Move, PlayerActionType.WallGrab);
        _player.PlayerActionLock(true, PlayerActionType.Jump, PlayerActionType.Move);
        _player.VeloCityResetImm(true, true);
        Vector2 dashVector = Vector2.zero;
        _curDashCount++;
        if (slide)
        {
            _player.ColliderSet(PlayerColliderType.Dash);
            _player.PlayerAnimation.SlideAnimation();
            _player.GravityModule.UseGravity = true;
            dashVector = _player.PlayerInput.InputVector * _player.playerMovementSO.dashPower;
            dashVector.y = 0f;
        }
        else
        {
            _player.PlayerAnimation.DashAnimation(_player.PlayerInput.InputVectorNorm);
            _player.GravityModule.UseGravity = false;
            dashVector = _player.PlayerInput.InputVectorNorm * _player.playerMovementSO.dashPower;
        }
        _player.VelocitySetExtra(dashVector.x);
        _player.AfterImageEnable(true);
        yield return new WaitForSeconds(_player.playerMovementSO.dashContinueTime);
        DashExit();
    }

    private IEnumerator DashChargeCoroutine()
    {
        yield return new WaitForSeconds(_player.playerMovementSO.dashChargeTime);
        _curDashCount = 0;
    }

    public void DashExit()
    {
        _excuting = false;
        _player.ColliderSet(PlayerColliderType.Normal);
        _player.AfterImageEnable(false);
        _player.PlayerActionLock(false, PlayerActionType.Jump, PlayerActionType.Move);
        _player.GravityModule.UseGravity = true;
        _player.VelocitySetExtra(0f, 0f);
        if (_player.PlayerActionCheck(PlayerActionType.WallGrab) == false)
        {
            _player.PlayerAnimation.FallOrIdleAnimation(_player.IsGrounded);
        }
        OnDashEnded?.Invoke(_player.IsGrounded);
        if (_player.IsGrounded)
        {
            DashCharge();
        }
    }

    public void OnGrounded(bool value)
    {
        if (value == false)
            return;
        _curDashCount = 0;
        DashCharge();
    }

    private void DashCharge()
    {
        if (_dashChargeCoroutine != null)
            StopCoroutine(_dashChargeCoroutine);
        _dashChargeCoroutine = StartCoroutine(DashChargeCoroutine());
    }

    public override void ActionExit()
    {
        _excuting = false;
        if (_dashCoroutine != null)
        {
            StopCoroutine(_dashCoroutine);
        }
        DashExit();
    }
}
