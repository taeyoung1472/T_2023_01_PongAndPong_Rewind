using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWallGrab : MonoBehaviour
{
    private bool _wallGrabed = false;
    public bool WallGrabed => _wallGrabed;

    [SerializeField]
    private LayerMask _wallMask = 0;
    [SerializeField]
    private float _rayLength = 0.05f;
    [SerializeField]
    private PlayerMovementSO _playerMovementSO = null;
    [SerializeField]
    private UnityEvent<bool> OnWallGrabed = null;

    private Player _player = null;
    private Rigidbody _rigid = null;
    private Coroutine _moveCo = null;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        WallCheck();
    }

    public void WallExit()
    {
        _wallGrabed = false;
        _player.GravityModule.GravityScale = _player.GravityModule.OriginGravityScale;
        _player.PlayerAllActionSet(true);

        if (_moveCo != null)
            StopCoroutine(_moveCo);
        _moveCo = StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        _player.PlayerMove.Moveable = false;
        yield return new WaitForSeconds(_playerMovementSO.wallGrabJumpContinueTime);
        _player.PlayerMove.Moveable = true;
    }

    private void WallCheck()
    {
        bool oldCheck = _wallGrabed;
        _wallGrabed = Physics.BoxCast(transform.position, transform.lossyScale * 0.5f, _player.PlayerRenderer.Fliped ? transform.right * -1f : transform.right, transform.rotation, _rayLength, _wallMask);
        if (oldCheck == _wallGrabed)
            return;
        if(_wallGrabed)
        {
            _player.GravityModule.UseGravity = true;
            _rigid.velocity = Vector3.zero;
            _player.GravityModule.GravityScale = _playerMovementSO.wallSlideGravityScale;
            _player.PlayerAllActionSet(false);
            _player.PlayerJump.Jumpable = true;
        }
        else
        {
            _player.GravityModule.GravityScale = _player.GravityModule.OriginGravityScale;
            _player.PlayerAllActionSet(true);
            _player.PlayerAnimation.FallOrIdleAnimation(_player.PlayerJump.IsGrounded);
        }
        OnWallGrabed?.Invoke(_wallGrabed);
    }
}
