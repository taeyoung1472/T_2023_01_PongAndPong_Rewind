using UnityEngine;

public class PlayerInteract : PlayerAction, IPlayerResetable
{
    private Interact _curInteract = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Interact"))
        {

            _curInteract = other.GetComponentInParent<Interact>();
            _curInteract.InteractEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
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

    public void EnableReset()
    {
    }

    public void DisableReset()
    {
        _curInteract = null;
    }
}
