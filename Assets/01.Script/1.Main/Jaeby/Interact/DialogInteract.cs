using System.Collections.Generic;
using UnityEngine;

public class DialogInteract : Interact
{
    [SerializeField]
    private DialogDataSO _curDialogData = null;
    [SerializeField]
    private List<DialogOptionDataSO> _dialogOptions = new List<DialogOptionDataSO>();
    private NPC _myNPC = null;

    [SerializeField]
    protected Animator _dialogAnimator = null;

    private void Start()
    {
        _myNPC ??= GetComponentInParent<NPC>();
    }

    protected override void ChildInteractEnd()
    {
        _curDialogData = _curDialogData.nextData;
        _interactable = _curDialogData != null;
        if (_interactable)
            InteractEnter();
    }

    protected override void ChildInteractStart()
    {
        InteractExit();
        if (DialogManager.Instance.DialogStart(_myNPC.npcData, this, _curDialogData, _dialogOptions, EndAction))
        {
            if (_dialogAnimator != null)
            {
                _dialogAnimator.Play("DialogStart");
                _dialogAnimator.Play("DialogStartUpBody");
                _dialogAnimator.Update(0);
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
