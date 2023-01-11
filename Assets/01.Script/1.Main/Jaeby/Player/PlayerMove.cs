using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PlayerMove : MonoBehaviour
{
    private bool _moveable = true;
    public bool Moveable { get => _moveable; set => _moveable = value; }

    private Rigidbody _rigid = null;
    private Vector2 _moveVelocity = Vector2.zero;

    [SerializeField]
    private PlayerMovementSO _playerMovementSO = null;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 dir)
    {
        if (_moveable == false)
            return;
        dir.y = 0f;
        _moveVelocity = dir.normalized * _playerMovementSO.speed;
    }

    private void FixedUpdate()
    {
        if (_moveable == false)
            return;
        _moveVelocity.y = _rigid.velocity.y;
        _rigid.velocity = _moveVelocity;
    }
}
