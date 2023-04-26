using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWallGrab : PlayerAction, IPlayerResetable
{
    [SerializeField]
    private UnityEvent<bool> OnWallGrabed = null;
    [SerializeField]
    private float _wallgrabCooltime = 0.4f;
    private Coroutine _wallGrabCoroutine = null;

    public void WallEnter(GameObject wallObj, Vector3 wallPosition)
    {
        if (_locked)
            return;
        _excuting = true;

        _player.ForceStop();
        _player.PlayerActionLock(true, PlayerActionType.Dash, PlayerActionType.Move, PlayerActionType.WallGrab);
        _player.PlayerRenderer.Flip(wallPosition - _player.transform.position);
        _player.GravityModule.UseGravity = false;
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
        WallExit();

        if (_wallGrabCoroutine != null)
            StopCoroutine(_wallGrabCoroutine);
        _player.PlayerActionLock(false, PlayerActionType.WallGrab, PlayerActionType.Move, PlayerActionType.Dash);
    }

    public void EnableReset()
    {
    }

    public void DisableReset()
    {
    }
}
