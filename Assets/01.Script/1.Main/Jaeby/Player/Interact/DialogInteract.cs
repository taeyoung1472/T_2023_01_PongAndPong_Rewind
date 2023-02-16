using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogInteract : Interact
{
    [SerializeField]
    private GameObject _canvas = null;
    [SerializeField]
    private TextMeshProUGUI _text = null;
    [SerializeField]
    private TextMeshProUGUI _doInteractText = null;

    [SerializeField]
    private DialogDataSO _curDialogData = null;

    private void Start()
    {
        _canvas.SetActive(false);
    }

    public override void InteractEnd(Player player)
    {
        _canvas.SetActive(false);
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
        _canvas.SetActive(true);
        DialogManager.Instance.DialogStart(_text, _curDialogData,
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
        _canvas.SetActive(true);
        _doInteractText.SetText($"{KeyManager.keys[InputType.Attack]}키를 눌러 상호작용 하세요.");
    }

    public override void InteractExit()
    {
        _canvas.SetActive(false);
        _doInteractText.SetText("");
    }
}
