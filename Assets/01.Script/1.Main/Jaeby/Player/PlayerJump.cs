using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerJump : MonoBehaviour
{
    [SerializeField]
    private float _jumpContinueTime = 0.2f;
    [SerializeField]
    private int _jumpCount = 1;
    private int _curJumpCount = 0;
    [SerializeField]
    private float _jumpPower = 2f;
    [SerializeField]
    private float _raySize = 0.1f;
    [SerializeField]
    private LayerMask _mask = 0;
    [SerializeField]
    private UnityEvent<bool> OnGroundCheck = null;
    [SerializeField]
    private UnityEvent OnJump = null;

    private Rigidbody _rigid = null;

    private bool _isGrounded = false;
    public bool IsGrounded => _isGrounded;
    private bool _isJumped = false;
    private bool _jumpable = true;
    public bool Jumpable { get => _jumpable; set => _jumpable = value; }
    private float _jumpTimer = 0f;
    private bool _jumpEndCheck = false;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public void JumpStart()
    {
        if (_curJumpCount >= _jumpCount || _jumpable == false)
            return;
        _curJumpCount++;
        _isJumped = true;
        _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpPower);
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

    private void GroundCheck()
    {
        if (_rigid.velocity.y > 0)
        {
            _isGrounded = false;
            OnGroundCheck?.Invoke(_isGrounded);
            return;
        }

        _isGrounded = Physics.BoxCast(transform.position, transform.lossyScale * 0.5f, transform.up * -1f, transform.rotation, _raySize, _mask);

        if(_isGrounded)
        {
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
            if (_jumpTimer >= _jumpContinueTime)
            {
                JumpEnd();
            }
        }
    }
}
