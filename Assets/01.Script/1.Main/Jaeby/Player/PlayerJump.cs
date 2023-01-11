using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerJump : MonoBehaviour
{
    private bool _jumpable = true;
    public bool Jumpable { get => _jumpable; set => _jumpable = value; }

    [SerializeField]
    private PlayerMovementSO _playerMovementSO = null;
    private int _curJumpCount = 0;
    [SerializeField]
    private float _raySize = 0.1f;
    [SerializeField]
    private LayerMask _mask = 0;
    [SerializeField]
    private UnityEvent<bool> OnGroundCheck = null;
    [SerializeField]
    private UnityEvent OnJump = null;
    [SerializeField]
    private UnityEvent OnWallGrabJump = null;
    private Rigidbody _rigid = null;
    private Player _player = null;

    private bool _isGrounded = false;
    public bool IsGrounded => _isGrounded;
    private bool _isJumped = false;
    private float _jumpTimer = 0f;
    private bool _jumpEndCheck = false;

    private void Start()
    {
        _player = GetComponent<Player>();
        _rigid = GetComponent<Rigidbody>();
    }

    public void JumpStart()
    {
        if (_jumpable == false)
            return;
        if (_player.PlayerWallGrab.WallGrabed)
        {
            OnWallGrabJump?.Invoke();
            ForceJump(_player.PlayerRenderer.Fliped ? new Vector2(1, 1) : new Vector2(-1, 1));
            _player.PlayerRenderer.ForceFlip();
            return;
        }
        if (_curJumpCount >= _playerMovementSO.jumpCount)
            return;
        _curJumpCount++;
        _isJumped = true;
        _jumpTimer = 0f;
        _rigid.velocity = new Vector2(_rigid.velocity.x, _playerMovementSO.jumpPower);
        OnJump?.Invoke();
    }

    public void JumpEnd()
    {
        if (_jumpEndCheck)
            return;
        _jumpEndCheck = true;
        _isJumped = false;
        _jumpTimer = 0f;
        _rigid.velocity = new Vector2(_rigid.velocity.x, 0f);
    }

    public void MoreJump()
    {
        JumpEnd();
        _curJumpCount--;
        _curJumpCount = Mathf.Clamp(_curJumpCount, 0, _playerMovementSO.jumpCount);
    }

    public void ForceJump(Vector2 dir)
    {
        dir.Normalize();
        _rigid.AddForce(dir * _playerMovementSO.wallGrabJumpPower, ForceMode.Impulse);
        OnJump?.Invoke();
    }

    public void TryGravityUp(Vector2 input)
    {
        if (_isGrounded || input.y > -0.1f)
            return;
        _player.GravityModule.GravityScale = _playerMovementSO.downGravityScale;
    }

    private void GroundCheck()
    {
        if (_rigid.velocity.y > 0)
        {
            _isGrounded = false;
            OnGroundCheck?.Invoke(_isGrounded);
            return;
        }

        _isGrounded = Physics.BoxCast(transform.position, transform.lossyScale * 0.5f, transform.up * -1f, transform.rotation, _raySize, _mask);

        if (_isGrounded)
        {
            _player.GravityModule.GravityScale = _player.GravityModule.OriginGravityScale;
            _curJumpCount = 0;
            _jumpEndCheck = false;
        }
        OnGroundCheck?.Invoke(_isGrounded);
    }

    private void Update()
    {
        GroundCheck();
        if (_isJumped)
        {
            _jumpTimer += Time.deltaTime;
            if (_jumpTimer >= _playerMovementSO.jumpContinueTime)
            {
                JumpEnd();
            }
        }
    }
}
