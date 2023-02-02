using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    private Transform _rayStartTrm = null;
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
    public bool IsJumped => _isJumped;
    private float _jumpTimer = 0f;
    private bool _jumpEndCheck = false;

    private void Start()
    {
        _player = GetComponent<Player>();
        _rigid = GetComponent<Rigidbody>();
    }

    public void JumpStart()
    {
        if (_jumpable == false || _player.PlayerAttack.Attacking)
            return;
        if (_player.PlayerWallGrab.WallGrabed)
        {
            ForceJump(_player.PlayerRenderer.Fliped ? new Vector2(1, 1) : new Vector2(-1, 1));
            _player.PlayerRenderer.ForceFlip();
            OnWallGrabJump?.Invoke();
            return;
        }
        if (_curJumpCount >= _playerMovementSO.jumpCount)
            return;

        _curJumpCount++;
        _jumpEndCheck = false;
        _isJumped = true;
        _jumpTimer = 0f;
        _rigid.velocity = new Vector2(_rigid.velocity.x, 0f);
        _rigid.AddForce(Vector3.up * _playerMovementSO.jumpPower, ForceMode.Impulse);
        OnJump?.Invoke();
    }


    public void JumpEnd()
    {
        if (_jumpEndCheck)
            return;

        _jumpEndCheck = true;
        _isJumped = false;
        _jumpTimer = 0f;
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
        if (_isGrounded || _player.PlayerWallGrab.WallGrabed || input.y > -0.1f)
            return;
        _player.GravityModule.GravityScale = _playerMovementSO.downGravityScale;
    }

    private void GroundCheck()
    {
        bool prevCheck = _isGrounded;
        _isGrounded = Physics.BoxCast(_rayStartTrm.position - Vector3.down * _raySize, _rayStartTrm.lossyScale * 0.5f, Vector3.down, transform.rotation, _raySize, _mask);
        Debug.Log(_isGrounded);
        if (prevCheck == _isGrounded)
            return;
        if (_isGrounded)
        {
            _player.GravityModule.GravityScale = _player.GravityModule.OriginGravityScale;
            _curJumpCount = 0;
        }
        OnGroundCheck?.Invoke(_isGrounded);
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireCube(_rayStartTrm.position, _rayStartTrm.lossyScale);
    }

    private void Update()
    {
        if (_isJumped)
        {
            _jumpTimer += Time.deltaTime;
            if (_jumpTimer >= _playerMovementSO.jumpContinueTime)
            {
                JumpEnd();
            }
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
        if (_isJumped)
        {
            if(_player.PlayerWallGrab.WallGrabed == false)
                _rigid.AddForce(Vector3.up * _playerMovementSO.jumpHoldPower, ForceMode.Acceleration);
        }
    }
}
