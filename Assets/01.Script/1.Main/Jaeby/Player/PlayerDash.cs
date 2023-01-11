using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDash : MonoBehaviour
{
    private bool _dashable = true;
    public bool Dashable { get => _dashable; set => _dashable = value; }

    [SerializeField]
    private PlayerMovementSO _playerMovementSO = null;
    private int _curDashCount = 0;
    [SerializeField]
    private UnityEvent<Vector2> OnDashStarted = null;
    [SerializeField]
    private UnityEvent<bool> OnDashEnded = null;

    private float _curDashTime = 0f;
    private bool _dashed = false;
    private Vector2 _inputDir = Vector2.zero;

    private Rigidbody _rigid = null;
    private Player _player = null;

    private void Start()
    {
        _player = GetComponent<Player>();
        _rigid = GetComponent<Rigidbody>();
    }

    public void DashDirSet(Vector2 input)
    {
        _inputDir = input.normalized;
    }

    public void Dash()
    {
        if (_dashable == false || _curDashCount >= _playerMovementSO.dashCount || _inputDir.sqrMagnitude == 0f)
            return;
        _curDashCount++;
        _player.PlayerMove.Moveable = false;
        _dashable = false;
        _dashed = true;
        _player.GravityModule.UseGravity = false;
        _rigid.velocity = _inputDir * _playerMovementSO.dashPower;
        OnDashStarted?.Invoke(_inputDir);
    }

    public void DashExit()
    {
        _curDashTime = 0f;
        _player.GravityModule.UseGravity = true;
        _dashed = false;

        if (_player.PlayerWallGrab.WallGrabed)
            return;

        _player.PlayerMove.Moveable = true;
        OnDashEnded?.Invoke(_player.PlayerJump.IsGrounded);
        _rigid.velocity = Vector3.zero;
    }

    public void DashCountReset(bool isGrounded)
    {
        if (isGrounded == false)
            return;

        _dashable = true;
        _curDashCount = 0;
    }

    private void Update()
    {
        if(_dashed)
        {
            _curDashTime += Time.deltaTime;
            if (_curDashTime >= _playerMovementSO.dashContinueTime)
                DashExit();
        }
    }
}
