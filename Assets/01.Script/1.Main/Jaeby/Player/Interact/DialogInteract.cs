using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogInteract : Interact
{
    [SerializeField]
    private DialogDataSO _curDialogData = null;
    [SerializeField]
    private List<DialogOption> _dialogOptions = new List<DialogOption>();
    [SerializeField]
    private GameObject _doInteractIcon = null;
    private NPC _myNPC = null;

    private void Start()
    {
        if (_doInteractIcon != null)
            _doInteractIcon.SetActive(false);
        _myNPC ??= GetComponentInParent<NPC>();
    }

    protected override void ChildInteractEnd()
    {
        _curDialogData = _curDialogData.nextData;
        if (_curDialogData != null)
            InteractEnter();
    }

    protected override void ChildInteractStart()
    {
        if (_curDialogData == null)
            return;
        InteractExit();
        DialogManager.Instance.DialogStart(_myNPC.npcData, this, _curDialogData, _dialogOptions,
            () =>
            {
                if (_dialogOptions.Count == 0)
                {
                    if (_chainInteract != null)
                    {
                        _chainInteract.Init(_player);
                        _chainInteract.InteractStart();
                    }
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
