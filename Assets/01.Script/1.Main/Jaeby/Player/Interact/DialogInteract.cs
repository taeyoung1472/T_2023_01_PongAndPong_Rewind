using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogInteract : Interact
{
    [SerializeField]
    private DialogDataSO _curDialogData = null;
    [SerializeField]
    private GameObject _doInteractIcon = null;

    private void Start()
    {
        if (_doInteractIcon != null)
            _doInteractIcon.SetActive(false);
    }

    public override void InteractEnd(Player player)
    {
        _curDialogData = _curDialogData.nextData;
        if (_curDialogData != null)
            InteractEnter();

        if(_chainInteract == null)
            player.PlayerActionExit(PlayerActionType.Interact);
    }

    public override void InteractStart(Player player)
    {
        if (_interactable == false || _curDialogData == null)
            return;
        InteractExit();
        player.VeloCityResetImm(true, true);
        DialogManager.Instance.DialogStart(_curDialogData,
            () =>
            {
                InteractEnd(player);
                if (_chainInteract != null)
                {
                    _chainInteract.Init(player);
                    _chainInteract.InteractStart();
                }
            }
            );
    }

    public void DialogChange(DialogDataSO data)
    {
        _curDialogData = data;
    }

    public override void InteractEnter()
    {
        if (_doInteractIcon != null)
            _doInteractIcon.SetActive(true);
    }

    public override void InteractExit()
    {
        if (_doInteractIcon != null)
            _doInteractIcon.SetActive(false);
    }
}
