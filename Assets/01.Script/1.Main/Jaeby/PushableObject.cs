using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private Player _player = null;
    private Rigidbody _rigid = null;
    private float _myMass = 1f;
    private float _curMass = 1f;

    private bool _pushing = false;

    private void Start()
    {
        _rigid = transform.parent.GetComponent<Rigidbody>();
        _myMass = _rigid.mass;
        _curMass = _myMass;
    }

    private void Update()
    {
        MassChange();
        CalculatePush();
    }

    private void CalculatePush()
    {
        if (_pushing == false)
            return;
        _rigid.MovePosition(transform.position + _player.PlayerRenderer.Forward * _curMass * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == false)
            return;

        _player = other.GetComponent<Player>();
        _pushing = true;
        other.GetComponent<Player>().GetPlayerAction<PlayerObjectPush>(PlayerActionType.ObjectPush).PushStart(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") == false)
            return;

        _pushing = false;
        other.GetComponent<Player>().GetPlayerAction<PlayerObjectPush>(PlayerActionType.ObjectPush).PushEnd(gameObject);
    }

    private void MassChange()
    {
        if (_pushing == false)
            return;
        if (_player.PlayerActionCheck(PlayerActionType.Dash))
        {
            _curMass = 0f;
        }
        else
        {
            _curMass = _myMass;
        }
    }
}
