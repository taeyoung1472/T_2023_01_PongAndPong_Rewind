using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class DialogManager : MonoSingleTon<DialogManager>
{
    private Coroutine _dialogCoroutine = null;
    private bool _excuting = false;
    private bool _input = false;
    private StringBuilder _sb = null;

    private void Start()
    {
        _sb = new StringBuilder();
    }

    public void DialogStart(TextMeshProUGUI text, DialogDataSO data, Action Callback = null)
    {
        if (_excuting)
        {
            StopCoroutine(_dialogCoroutine);
            _excuting = false;
            _input = false;
        }

        _dialogCoroutine = StartCoroutine(DialogCoroutine(text, data, Callback));
    }

    private IEnumerator DialogCoroutine(TextMeshProUGUI text, DialogDataSO data, Action Callback = null)
    {
        _excuting = true;
        _sb.Clear();
        for (int i = 0; i < data.texts.Count; i++)
        {
            string targetText = data.texts[i];
            for (int j = 0; j < targetText.Length; j++)
            {
                if(_input)
                {
                    _input = false;
                    text.SetText(targetText);
                    break;
                }
                _sb.Append(targetText[j]);
                text.SetText(_sb.ToString());
                yield return new WaitForSeconds(data.nextCharDelay);
            }
            yield return new WaitUntil(() => _input);
            _input = false;
            _sb.Clear();
        }
        text.SetText("");
        _excuting = false;
        Callback?.Invoke();
    }

    private void Update()
    {
        if (_excuting == false || _input)
            return;
        if(Input.GetKeyDown(KeyManager.keys[InputType.Jump]))
        {
            _input = true;
        }
    }
}
