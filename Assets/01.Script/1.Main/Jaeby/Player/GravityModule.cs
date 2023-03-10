using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModule : MonoBehaviour
{
    [SerializeField]
    private float _originGravity = 1f;
    public float OriginGravityScale { get => _originGravity; set => _originGravity = value; }

    [SerializeField]
    private float _maxGravityAcceleration = 1f;
    public float MaxGravityAcceleration { get => _maxGravityAcceleration; set => _maxGravityAcceleration = value; }

    private float _gravityScale = 1f;
    public float GravityScale { get => _gravityScale; set => _gravityScale = value; }

    [SerializeField]
    private bool _useGravity = true;
    public bool UseGravity { get => _useGravity; set => _useGravity = value; }

    private float _curGravityAcceleration = 0f;
    public float CurGravityAcceleration => _curGravityAcceleration;
    private Coroutine _accelCo = null;

    private void Start()
    {
        _gravityScale = _originGravity;
    }

    public Vector3 GetGravity()
    {
        if (_useGravity)
        {
            return Physics.gravity * (_gravityScale + _curGravityAcceleration);
        }
        else
        {
            return Vector3.zero;
        }
    }

    public void OnGrounded(bool val)
    {
        if (val)
        {
            if (_accelCo != null)
                StopCoroutine(_accelCo);
            _curGravityAcceleration = 0f;
        }
        else
        {
            if (_accelCo != null)
                StopCoroutine(_accelCo);
            _accelCo = StartCoroutine(AccelerationCoroutine());
        }
    }

    private IEnumerator AccelerationCoroutine()
    {
        float x = 0f;
        while (x <= 1f)
        {
            x += Time.deltaTime;
            _curGravityAcceleration = _maxGravityAcceleration * x;
            yield return null;
        }
        _curGravityAcceleration = _maxGravityAcceleration;
    }
}
