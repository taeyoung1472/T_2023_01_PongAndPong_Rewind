using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private Player _player = null;
    private Rigidbody _rigid = null;
    private float _myMass = 1f;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _myMass = _rigid.mass;
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
        other.GetComponent<Player>().GetPlayerAction<PlayerObjectPush>(PlayerActionType.ObjectPush).PushStart(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") == false)
            return;
        other.GetComponent<Player>().GetPlayerAction<PlayerObjectPush>(PlayerActionType.ObjectPush).PushEnd(gameObject);
    }

    private void MassChange()
    {
        if (_player != null)
        {
            if (_player.PlayerActionCheck(PlayerActionType.Dash))
            {
                _rigid.mass = 90f;
            }
            else
            {
                _rigid.mass = _myMass;
            }
        }
    }
}
