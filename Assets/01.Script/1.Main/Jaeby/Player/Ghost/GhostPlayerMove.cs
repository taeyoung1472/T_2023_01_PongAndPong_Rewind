using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlayerMove : MonoBehaviour
{
    [SerializeField]
    private GhostPlayerDataSO _ghostPlayerDataSO = null;
    private bool _moveable = true;
    public bool Moveable { get => _moveable; set => _moveable = value; }
    private Rigidbody _rigid = null;
    private Vector2 _moveVector = Vector2.zero;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 dir)
    {
        dir.Normalize();
        _moveVector = dir * _ghostPlayerDataSO.speed;
    }

    private void FixedUpdate()
    {
        _rigid.velocity = _moveVector;
    }

    public void StopImm()
    {
        _rigid.velocity = Vector2.zero;
    }
}
