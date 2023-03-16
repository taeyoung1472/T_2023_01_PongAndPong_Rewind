using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : PlayerAction
{
    private Interact _curInteract = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Interact"))
        {
            Debug.Log("닿았어");
            _curInteract = other.GetComponentInParent<Interact>();
            _curInteract.InteractEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("빠졌어");
        _curInteract?.InteractExit();
        _curInteract = null;
    }

    public bool TryInteract()
    {
        if (_curInteract == null || _locked)
            return false;
        _excuting = true;
        _player.PlayerInput.enabled = false;
        _player.ForceStop();
        _curInteract.InteractStart(_player);
        return true;
    }

    public override void ActionExit()
    {
        _player.PlayerInput.enabled = true;
        _excuting = false;
    }
}
