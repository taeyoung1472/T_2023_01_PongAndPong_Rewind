using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerWallGrab : PlayerAction
{
    [SerializeField]
    private UnityEvent<bool> OnWallGrabed = null;
    [SerializeField]
    private float _wallgrabCooltime = 0.4f;
    [SerializeField]
    private float _climbTime = 0.5f;
    private Coroutine _wallGrabCoroutine = null;

    public void WallClimb(Vector3 endPos, Vector3 wallPosition)
    {
        if (_locked || _player.IsGrounded)
            return;
        if (Vector3.Dot((wallPosition - _player.transform.position).normalized, _player.PlayerRenderer.Forward) < 0f)
            return;

        _player.PlayerRenderer.Flip(wallPosition - _player.transform.position, false);
        bool gravity = _player.GravityModule.UseGravity;
        _player.PlayerInput.enabled = false;
        _player.GravityModule.UseGravity = false;
        _player.ForceStop();
        Sequence climbSeq = DOTween.Sequence();
        climbSeq.Append(_player.transform.DOMoveY(endPos.y, _climbTime * 0.8f));
        climbSeq.Append(_player.transform.DOMoveX(endPos.x, _climbTime * 0.2f));
        climbSeq.AppendCallback(() =>
        {
            _player.GravityModule.UseGravity = gravity;
            _player.PlayerInput.enabled = true;
            WallExit();
        });
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
        yield return new WaitForSeconds(_wallgrabCooltime);
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
    }
}
