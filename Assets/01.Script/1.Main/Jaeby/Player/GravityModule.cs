using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModule : MonoBehaviour
{
    private CharacterController _characterController = null;

    private float _originGravity = 1f;
    public float OriginGravityScale { get => _originGravity; set => _originGravity = value; }

    [SerializeField]
    private float _gravityScale = 1f;
    public float GravityScale { get => _gravityScale; set => _gravityScale = value; }

    [SerializeField]
    private bool _useGravity = true;
    public bool UseGravity { get => _useGravity; set => _useGravity = value; }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _originGravity = _gravityScale;
    }

    public Vector3 GetGravity()
    {
        if (_characterController.isGrounded == false && _useGravity)
        {
            return Physics.gravity * _gravityScale;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
