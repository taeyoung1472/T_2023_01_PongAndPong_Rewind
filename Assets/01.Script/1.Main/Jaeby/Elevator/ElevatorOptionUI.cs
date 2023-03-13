using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorOptionUI : MonoBehaviour
{
    private int _myIndex = 0;
    public int Myindex { get => _myIndex; set => _myIndex = value; }
    private bool _locked = false;
    public bool Locked { get => _locked; set { _locked = value; _lockImage.enabled = _locked; } }
    private Image _lockImage = null;
    private TextMeshProUGUI _text = null;
    private string _originString = "";

    private void Start()
    {
        _lockImage = transform.GetComponentInChildren<Image>();
        _lockImage.enabled = _locked;
        _text = transform.GetComponentInChildren<TextMeshProUGUI>();
        _originString = _text.text;
    }

    public void ButtonMapping()
    {
        transform.GetComponentInChildren<Button>().onClick.AddListener(
            () =>
            {
                ElevatorManager.Instance.TargetElevatorSet(_myIndex);
            });
    }

    public void TextChange(int index)
    {
        if(index == _myIndex)
        {
            _text.SetText("(현재 위치)     " + _originString);
        }
        else
        {
            _text.SetText(_originString);
        }
    }
}
