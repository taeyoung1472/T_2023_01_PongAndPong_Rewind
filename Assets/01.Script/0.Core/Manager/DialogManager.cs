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
    private GameObject _dialogCanvas = null;
    [SerializeField]
    private TextMeshProUGUI _nameText = null;
    [SerializeField]
    private TextMeshProUGUI _titleText = null;
    [SerializeField]
    private TextMeshProUGUI _dialogText = null;
    [SerializeField]
    private GameObject _optionPrefab = null;
    [SerializeField]
    private Transform _optionParentTrm = null;
    [SerializeField]
    private DesignDataSO _designDataSO = null;

    private List<GameObject> _curOptions = new List<GameObject>();

    private void Start()
    {
        _sb = new StringBuilder();
        _dialogCanvas.SetActive(false);
    }

    public void DialogStart(NPCData npcData, DialogInteract dialogInteract, DialogDataSO data, List<DialogOption> dialogOptions, Action Callback = null)
    {
        if (_excuting)
        {
            StopCoroutine(_dialogCoroutine);
            _excuting = false;
            _input = false;
        }
        if (npcData != null)
        {
            _nameText.SetText(npcData.npcName);
            _titleText.SetText("< " + _designDataSO.GetTitle(npcData.iconType) + " >");
            _titleText.color = _designDataSO.GetColor(npcData.npcType);
        }
        else
        {
            _nameText.SetText("");
            _titleText.SetText("");
        }

        _dialogCanvas.SetActive(true);
        _dialogCoroutine = StartCoroutine(DialogCoroutine(dialogInteract, data, dialogOptions, Callback));
    }

    private void DialogEnd()
    {
        _dialogText.SetText("");
        _excuting = false;
        _dialogCanvas.SetActive(false);
    }

    private IEnumerator DialogCoroutine(DialogInteract dialogInteract, DialogDataSO data, List<DialogOption> dialogOptions, Action Callback = null)
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
                Button button = Instantiate(_optionPrefab, _optionParentTrm).GetComponent<Button>();
                _curOptions.Add(button.gameObject);
                button.onClick.AddListener(
                    () =>
                    {
                        dialogOptions[i].ExcuteAction?.Invoke();
                        for (int i = 0; i < _curOptions.Count; i++)
                            Destroy(_curOptions[i]);
                        _curOptions.Clear();
                        dialogInteract.InteractEnd();
                        DialogEnd();
                    });
            }
        }
        else
        {
            dialogInteract.InteractEnd();
            DialogEnd();
        }

        Callback?.Invoke();
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
