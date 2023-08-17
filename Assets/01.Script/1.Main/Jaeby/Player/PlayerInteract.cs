using UnityEngine;

public class PlayerInteract : PlayerAction, IPlayerDisableResetable
{
    private Interact _curInteract = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Interact"))
        {
            Interact interact = other.GetComponentInParent<Interact>();
            if (interact != null)
            {
                if (interact.Interactable == false || _curInteract == interact)
                    return;
                _curInteract?.InteractExit();
                _curInteract = interact;
                _curInteract.InteractEnter();
            }
        }
        if (other.CompareTag("AutoInteract"))
        {
            Interact interact = other.GetComponentInParent<Interact>();
            if (interact != null)
            {
                if (interact.Interactable == false || _curInteract == interact)
                    return;
                _curInteract?.InteractExit();
                _curInteract = interact;
                _curInteract.InteractEnter();
                TryInteract();
            }
            //Debug.Log("�浿");
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_curInteract == null)
            return;

        if (other.CompareTag("Interact"))
        {
            _curInteract.InteractExit();
            _curInteract = null;
        }
    }

    public bool TryInteract()
    {
        if (_curInteract == null || _locked)
            return false;
        if (_curInteract.Interactable == false)
            return false;

        _excuting = true;
        _player.ForceStop();
        _player.PlayerInput.enabled = false;
        _curInteract.InteractStart(_player);
        return true;
    }

    public override void ActionExit()
    {
        _player.PlayerInput.enabled = true;
        _excuting = false;
    }

    public void DisableReset()
    {
        _curInteract = null;
    }
}
