using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoSingleTon<DialogManager>
{
    private Coroutine _dialogCoroutine = null;
    private bool _excuting = false;
    private bool _input = false;
    private StringBuilder _sb = null;

    [SerializeField]
    private DialogOptionManager _dialogOptionEventManager = null;

    [SerializeField]
    private float _dialogCooltime = 0.6f;
    private bool _dialogLock = false;

    [SerializeField]
    private GameObject _dialogCanvas = null;
    [SerializeField]
    private TextMeshProUGUI _nameText = null;
    [SerializeField]
    private TextMeshProUGUI _titleText = null;
    [SerializeField]
    private TextMeshProUGUI _dialogText = null;
    [SerializeField]
    private DialogOptionUI _optionPrefab = null;
    [SerializeField]
    private Transform _optionParentTrm = null;
    [SerializeField]
    private DesignDataSO _designDataSO = null;

    private List<GameObject> _curOptions = new List<GameObject>();
    private NPCData _curNPCData = null;
    private DialogInteract _curDialogInteract = null;

    private void Start()
    {
        _sb = new StringBuilder();
        _dialogCanvas.SetActive(false);
    }

    public void DialogForceExit()
    {
        StopCoroutine(_dialogCoroutine);
        if (_curDialogInteract != null)
            _curDialogInteract.InteractEnd(true);
        for (int i = 0; i < _curOptions.Count; i++)
            Destroy(_curOptions[i]);
        _curOptions.Clear();
        DialogEnd();
    }

    public bool DialogStart(NPCData npcData, DialogInteract dialogInteract, DialogDataSO data, List<DialogOptionDataSO> dialogOptions, Action Callback = null)
    {
        if (_dialogLock)
            return false;

        if (npcData != null)
        {
            _nameText.SetText(npcData.npcName);
            _titleText.SetText("< " + _designDataSO.GetTitle(npcData.iconType) + " >");
            _titleText.color = _designDataSO.GetColor(npcData.npcType);
            _curNPCData = npcData;
        }
        else
        {
            _nameText.SetText("");
            _titleText.SetText("");
        }

        _dialogCanvas.SetActive(true);
        _curDialogInteract = dialogInteract;
        _dialogCoroutine = StartCoroutine(DialogCoroutine(dialogInteract, data, dialogOptions, Callback));
        return true;
    }

    private void DialogEnd()
    {
        _dialogText.SetText("");
        _excuting = false;
        _input = false;
        _curNPCData = null;
        _curDialogInteract = null;
        _dialogCanvas.SetActive(false);
        if(_dialogLock == false)
            StartCoroutine(DialogCooltimeCoroutine());
    }

    private IEnumerator DialogCooltimeCoroutine()
    {
        _dialogLock = true;
        yield return new WaitForSeconds(_dialogCooltime);
        _dialogLock = false;
    }

    private IEnumerator DialogCoroutine(DialogInteract dialogInteract, DialogDataSO data, List<DialogOptionDataSO> dialogOptions, Action Callback = null)
    {
        _excuting = true;
        _sb.Clear();
        for (int i = 0; i < data.texts.Count; i++)
        {
            string targetText = data.texts[i];
            for (int j = 0; j < targetText.Length; j++)
            {
                if (_input)
                {
                    _input = false;
                    _dialogText.SetText(targetText);
                    break;
                }
                _sb.Append(targetText[j]);
                _dialogText.SetText(_sb.ToString());
                yield return new WaitForSeconds(data.nextCharDelay);
            }
            yield return new WaitUntil(() => _input);
            _input = false;
            _sb.Clear();
        }
        // end
        if(dialogOptions.Count > 0)
        {
            for (int i = 0; i < dialogOptions.Count; i++)
            {
                ButtonSet(i, dialogInteract, dialogOptions);
            }
        }
        else
        {
            dialogInteract.InteractEnd(true);
            DialogEnd();
        }

        Callback?.Invoke();
    }

    private void ButtonSet(int index, DialogInteract dialogInteract, List<DialogOptionDataSO> dialogOptions)
    {
        DialogOptionUI optionUI = Instantiate(_optionPrefab, _optionParentTrm);
        Sprite icon = dialogOptions[index].icon;
        if (icon == null)
            icon = _dialogOptionEventManager.DefaultIcon;
        optionUI.Init(icon, dialogOptions[index].explainText, () =>
        {
            for (int i = 0; i < _curOptions.Count; i++)
                Destroy(_curOptions[i]);
            _curOptions.Clear();
            if (dialogOptions[index]._nextDialogData != null)
            {
                DialogDataSO dataSO = dialogOptions[index]._nextDialogData;
                DialogStart(_curNPCData, dialogInteract, dataSO, dialogOptions[index].dialogOptions, null);
            }
            else
            {
                dialogInteract.InteractEnd(dialogOptions[index].actionExit);
                DialogEnd();
            }
            _dialogOptionEventManager.GetEvent(dialogOptions[index].eventKey)?.Invoke();
        });
        _curOptions.Add(optionUI.gameObject);
    }

    private void Update()
    {
        if (_excuting == false || _input)
            return;
        if (Input.GetKeyDown(KeyCode.F))
        {
            _input = true;
        }
    }
}
