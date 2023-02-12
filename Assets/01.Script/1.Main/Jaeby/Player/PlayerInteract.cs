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
            _curInteract = other.GetComponent<Interact>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("빠졌어");
        _curInteract = null;
    }

    public void TryInteract()
    {
        if (_curInteract == null || _locked)
            return;
        _player.PlayerInput.InputVectorReset();
        _player.PlayerInput.enabled = false;
        _player.PlayerAnimation.FallOrIdleAnimation(_player.IsGrounded);
        _curInteract.InteractStart(_player);
    }

    public override void ActionExit()
    {
        _player.PlayerInput.enabled = true;
    }
}
