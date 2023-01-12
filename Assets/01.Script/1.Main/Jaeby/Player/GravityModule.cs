using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModule : MonoBehaviour
{
    private Rigidbody _rigid = null;

    private float _originGravity = 1f;
    public float OriginGravityScale { get => _originGravity; set => _originGravity = value; }

    [SerializeField]
    private float _gravityScale = 1f;
    public float GravityScale { get => _gravityScale; set => _gravityScale = value; }

    private bool _useGravity = true;
    public bool UseGravity { get => _useGravity; set => _useGravity = value; }

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _rigid.useGravity = false;
        _originGravity = _gravityScale;
    }

    private void FixedUpdate()
    {
        AddGravity();
    }

    private void AddGravity()
    {
        if (_useGravity == false)
            return;
        _rigid.AddForce(Physics.gravity * _gravityScale, ForceMode.Acceleration);
    }
}
