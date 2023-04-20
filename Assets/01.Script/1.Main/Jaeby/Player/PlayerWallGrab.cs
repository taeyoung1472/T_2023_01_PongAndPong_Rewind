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

    private GameObject _prevGrabObject;
    private float _prevCheckTimer;

    private void Update()
    {
        WallCheck();
        DrawDebugRay();
        CheckPrevGameObjcet();
    }

    private void CheckPrevGameObjcet()
    {
        if (_prevGrabObject)
        {
            _prevCheckTimer -= Time.deltaTime;
            if (_prevCheckTimer < 0)
            {
                _prevCheckTimer = 0.1f;
                _prevGrabObject = null;
            }
        }
    }

    private void DrawDebugRay()
    {
        Vector3 footPos = _player.transform.position + _player.Col.center + Vector3.down * _player.Col.bounds.size.y * 0.5f;

        Ray ray = new Ray(footPos, _player.PlayerRenderer.Down);
        Debug.DrawRay(ray.origin, ray.direction * ((_player.Col.radius) + _rayLength + _player.Col.contactOffset), Color.red);
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

        Vector3 footPos = _player.transform.position + _player.Col.center + Vector3.down * _player.Col.bounds.size.y * 0.5f;
        bool lastCheck = _excuting;
        Ray ray = new Ray(footPos, _player.PlayerRenderer.Down);
        if(Physics.Raycast(ray, out RaycastHit hit, _rayLength + _player.Col.contactOffset, _wallMask))
        {
            if (!_player.IsGrounded && _prevGrabObject != hit.transform.gameObject)
            {
                _excuting = true;
                _prevGrabObject = hit.transform.gameObject;
                _prevCheckTimer = 0.1f;
            }
        }
        if (lastCheck == _excuting) return;

        if (_excuting)
            WallGrabEnter();
        else
            WallGrabExit();
        OnWallGrabed?.Invoke(_excuting);
    }

    private void WallGrabExit() // 벽에서 나가!
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
