using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogInteract : Interact
{
    [SerializeField]
    private DialogDataSO _curDialogData = null;
    [SerializeField]
    private List<DialogOptionDataSO> _dialogOptions = new List<DialogOptionDataSO>();
    [SerializeField]
    private GameObject _doInteractIcon = null;
    private NPC _myNPC = null;
    protected Animator _animator = null;

    private void Start()
    {
        if (_doInteractIcon != null)
            _doInteractIcon.SetActive(false);
        _myNPC ??= GetComponentInParent<NPC>();
        _animator ??= GetComponentInParent<Animator>();
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
        bool result = DialogManager.Instance.DialogStart(_myNPC.npcData, this, _curDialogData, _dialogOptions,
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
        if (result == false)
        {
            InteractEnd(true);
        }
        else
        {
            if(_animator != null)
            {
                _animator.Play("DialogStart");
                _animator.Update(0);
            }
        }
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
