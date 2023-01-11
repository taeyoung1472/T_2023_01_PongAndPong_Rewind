using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField]
    private float _dashPower = 3f;
    [SerializeField]
    private int _dashCount = 1;
    private int _curDashCount = 0;
    [SerializeField]
    private float _dashContinueTime = 0.2f;
    private float _curDashTime = 0f;
    private bool _dashed = false;
    private bool _dashable = true;
    public bool Dashable { get => _dashable; set => _dashable = value; }
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
        if (_dashable == false || _curDashCount >= _dashCount)
            return;
        _curDashCount++;
        _player.Jumpable = false;
        _player.Moveable = false;
        _dashable = false;
        _dashed = true;
        _rigid.useGravity = false;
        _rigid.velocity = _inputDir * _dashPower;
    }

    public void DashExit()
    {
        _player.Jumpable = true;
        _player.Moveable = true;
        _dashed = false;
        _rigid.useGravity = true;
        _rigid.velocity = Vector3.zero;
        _curDashTime = 0f;
    }

    public void DashCountReset()
    {
        _dashable = true;
        _curDashCount = 0;
    }

    private void Update()
    {
        if(_dashed)
        {
            _curDashTime += Time.deltaTime;
            if (_curDashTime >= _dashContinueTime)
                DashExit();
        }
    }
}
