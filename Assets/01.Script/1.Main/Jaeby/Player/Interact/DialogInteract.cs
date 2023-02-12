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
    private DialogDataSO _curDialogData = null;

    private void Start()
    {
        _canvas.SetActive(false);
    }

    public override void InteractEnd(Player player)
    {
        _canvas.SetActive(false);
        _curDialogData = _curDialogData.nextData;
        player.PlayerActionExit(PlayerActionType.Interact);
    }

    public override void InteractStart(Player player)
    {
        if (_interactable == false || _curDialogData == null)
            return;
        player.VeloCityResetImm(true, true);
        _canvas.SetActive(true);
        DialogManager.Instance.DialogStart(_text, _curDialogData, () => { InteractEnd(player); });
    }

    public void DialogChange(DialogDataSO data)
    {
        _curDialogData = data;
    }
}
