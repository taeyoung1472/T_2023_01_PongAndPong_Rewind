using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2f;

    private Rigidbody _rigid = null;
    private Vector2 _moveVelocity = Vector2.zero;

    private bool _moveable = true;
    public bool Moveable { get => _moveable; set => _moveable = value; }

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 dir)
    {
        if (_moveable == false)
            return;
        dir.y = 0f;
        _moveVelocity = dir.normalized * _speed;
    }

    private void FixedUpdate()
    {
        if (_moveable == false)
            return;
        _moveVelocity.y = _rigid.velocity.y;
        _rigid.velocity = _moveVelocity;
    }
}
