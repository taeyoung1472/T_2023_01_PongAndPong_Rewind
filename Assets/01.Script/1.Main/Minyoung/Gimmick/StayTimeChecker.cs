using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StayTimeChecker : MonoBehaviour
{
    private Rigidbody _rigid = null;
    private float _stayTime = 0f;
    public float StayTime => _stayTime;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(_rigid.velocity.y < 0f)
        {
            _stayTime += Time.deltaTime;
        }
        else
        {
            _stayTime = 0f;
        }
    }


}
