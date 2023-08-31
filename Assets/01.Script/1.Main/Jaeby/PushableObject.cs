using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private Player _player = null;
    [SerializeField]
    private Rigidbody _rigid = null;

    private bool _pushing = false;

    private void Start()
    {
        _rigid = transform.parent.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        MassChange();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == false)
            return;

        _player = other.GetComponent<Player>();
        _pushing = true;
        other.GetComponent<Player>().GetPlayerAction<PlayerObjectPush>(PlayerActionType.ObjectPush).PushStart(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") == false || _player == null)
            return;

        if(_player.GetPlayerAction<PlayerObjectPush>(PlayerActionType.ObjectPush).IsObjExit(gameObject))
        {
            _player.GetPlayerAction<PlayerObjectPush>(PlayerActionType.ObjectPush).PushEnd(gameObject);
        }
        else
        {
            _player.GetPlayerAction<PlayerObjectPush>(PlayerActionType.ObjectPush).PushStart(gameObject);
        }
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
        if (_pushing == false || _rigid == null)
            return;
        if (_player.PlayerActionCheck(PlayerActionType.Dash, PlayerActionType.Jump))
        {
            _rigid.mass = 90f;
        }
        else
        {
            _rigid.mass = 0f;
        }
    }
}
