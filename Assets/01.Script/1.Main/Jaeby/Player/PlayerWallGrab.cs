using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWallGrab : PlayerAction, IPlayerDisableResetable
{
    [SerializeField]
    private UnityEvent<bool> OnWallGrabed = null;
    private Coroutine _wallGrabCoroutine = null;

    [SerializeField]
    private Transform _climbCameraTrm = null;

    private Sequence _climbSeq = null;

    private bool _gravity = false;
    private Vector3 _endPosition = Vector3.zero;

    public void WallClimb(Vector3 startPos, Vector3 endPos, Vector3 wallPosition)
    {
        if (_locked || _player.IsGrounded)
            return;
        if (Vector3.Dot((wallPosition - _player.transform.position).normalized, _player.PlayerRenderer.Forward) < 0f)
            return;

        _endPosition = endPos;
        _player.PlayerCameraControll(_climbCameraTrm, _player.transform);
        _climbCameraTrm.transform.position = startPos;
        _climbCameraTrm.DOMove(_endPosition, _player.playerMovementSO.climbTrmAnimateTime);
        _player.ForceStop();
        _player.transform.position = startPos;
        _excuting = true;
        _player.PlayerRenderer.Flip(wallPosition - _player.transform.position, false);
        _player.PlayerAnimation.WallClimbAnimation();
        _gravity = _player.GravityModule.UseGravity;
        _player.PlayerInput.enabled = false;
        _player.GravityModule.UseGravity = false;
    }

    public void ClimbAnimate()
    {
        if (_climbSeq != null)
            _climbSeq.Kill();
        _climbSeq = DOTween.Sequence();
        _climbSeq.Append(_player.PlayerAnimation.transform.DOMoveX(_endPosition.x, _player.playerMovementSO.climbAnimateTime));
    }

    public void WallClimbEnd()
    {
        WallExit();
        ModelPositionReset();
        _player.transform.position = _endPosition;
        _player.GravityModule.UseGravity = _gravity;
        _player.PlayerInput.enabled = true;

        _player.PlayerAnimation.Rebind();
        _player.PlayerAnimation.IdleAnimation();
        _player.PlayerCameraControll(_player.transform, _climbCameraTrm);
    }

    public void WallEnter(GameObject wallObj, Vector3 wallPosition)
    {
        if (_locked || _player.IsGrounded)
            return;
        if (Vector3.Dot((wallPosition - _player.transform.position).normalized, _player.PlayerRenderer.Forward) < 0f)
            return;

        _excuting = true;

        _player.ForceStop();
        _player.PlayerActionLock(true, PlayerActionType.Dash, PlayerActionType.Move, PlayerActionType.WallGrab);
        _player.PlayerRenderer.Flip(wallPosition - _player.transform.position, false);
        _player.GravityModule.UseGravity = false;
        _player.GetPlayerAction<PlayerJump>(PlayerActionType.Jump).MoreJump(1);
        OnWallGrabed?.Invoke(true);
    }

    public void WallExit()
    {
        _excuting = false;
        _player.PlayerActionLock(false, PlayerActionType.Dash);
        _player.GravityModule.UseGravity = true;
        if (_wallGrabCoroutine != null)
            StopCoroutine(_wallGrabCoroutine);
        _wallGrabCoroutine = StartCoroutine(WallGrabCoroutine());
        OnWallGrabed?.Invoke(false);
    }

    private IEnumerator WallGrabCoroutine()
    {
        yield return new WaitForSeconds(_player.playerMovementSO.wallgrabCooltime);
        _player.PlayerActionLock(false, PlayerActionType.WallGrab, PlayerActionType.Move);
    }

    public override void ActionExit()
    {
        _excuting = false;
        _player.GravityModule.UseGravity = true;
        if (_wallGrabCoroutine != null)
            StopCoroutine(_wallGrabCoroutine);
        _player.PlayerActionLock(false, PlayerActionType.WallGrab, PlayerActionType.Move, PlayerActionType.Dash);
        OnWallGrabed?.Invoke(false);
        _endPosition = Vector3.zero;
        _gravity = true;
        ModelPositionReset();
    }

    public void DisableReset()
    {
        Debug.Log("¾ÆÀ×");
        _player.PlayerCameraControll(null, _climbCameraTrm);
        _player.PlayerCameraControll(null, _player.transform);
    }

    private void ModelPositionReset()
    {
        _climbCameraTrm.transform.DOKill();
        _climbCameraTrm.transform.position = Vector3.zero;
        if (_climbSeq != null)
            _climbSeq.Kill();
        _player.PlayerAnimation.transform.localPosition = new Vector3(0f, 0.5f, 0f);
    }
}
