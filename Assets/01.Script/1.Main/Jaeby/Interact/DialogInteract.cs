using System.Collections.Generic;
using UnityEngine;

public class DialogInteract : Interact
{
    [SerializeField]
    private DialogDataSO _curDialogData = null;
    [SerializeField]
    private List<DialogOptionDataSO> _dialogOptions = new List<DialogOptionDataSO>();
    private NPC _myNPC = null;
    protected Animator _animator = null;

    private void Start()
    {
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
        if(DialogManager.Instance.DialogStart(_myNPC.npcData, this, _curDialogData, _dialogOptions, EndAction ))
        {
            if (_animator != null)
            {
                _animator.Play("DialogStart");
                _animator.Update(0);
            }
        }
        else
        {
            InteractEnd(true);
        }
    }

    private void EndAction()
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

    public void DialogChange(DialogDataSO data)
    {
        _curDialogData = data;
    }
}
