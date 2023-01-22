using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyMappingUI : MonoBehaviour
{
    KeyCode[] defaultKeys = new KeyCode[]
    {
        KeyCode.W,
        KeyCode.S,
        KeyCode.A,
        KeyCode.D,
        KeyCode.Space,
        KeyCode.Mouse0,
        KeyCode.LeftShift,
        KeyCode.Mouse1,
    };

    KeyCode[] cantKey = new KeyCode[]
    {
        KeyCode.Return,
        KeyCode.Escape
    };

    [SerializeField]
    private GameObject _mainPanel = null;
    [SerializeField]
    private GameObject _keyMappingPanel = null;
    [SerializeField]
    private GameObject _startButton = null;
    [SerializeField]
    private GameObject _mappingStartUI = null;

    [SerializeField]
    private GameObject _keyMappingPrefab = null;

    Event keyEvent;

    private bool _keyMapping = false;

    private Dictionary<InputType, KeyCode> _saveKeys = new Dictionary<InputType, KeyCode>();

    private void Start()
    {
        KeyManager.LoadKey();
        _saveKeys = KeyManager.keys;
        UIInit();
    }

    private void UIInit()
    {
        for (int i = 0; i < (int)InputType.Size; i++)
        {
            Button button = Instantiate(_keyMappingPrefab, _keyMappingPanel.transform).GetComponent<Button>();
            ButtonSet(button, i);
            TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
            text.SetText($"{((InputType)i).ToString()}");
        }
    }

    public void ButtonSet(Button button, int index)
    {
        button.onClick.AddListener(() => { KeyMappingStart((InputType)index); });
    }

    public void UIOn()
    {
        _mainPanel.SetActive(true);
        _startButton.SetActive(false);
    }

    public void UIOff()
    {
        _mainPanel.SetActive(false);
        _startButton.SetActive(true);
    }

    public void KeyMappingStart(InputType type)
    {
        if (_keyMapping)
            return;
        _mappingStartUI.SetActive(true);
        _mappingStartUI.GetComponentInChildren<TextMeshProUGUI>().SetText($"���ϴ� ��ư��\n�س��� ��������\n<��ư : {type.ToString()}>");
        StartCoroutine(KeyMappingCoroutine(type));
    }

    public void KeyMappingEnd()
    {
        _mappingStartUI.SetActive(false);
    }

    private IEnumerator KeyMappingCoroutine(InputType type)
    {
        _keyMapping = true;
        KeyCode value = KeyCode.None;
        while (true)
        {
            yield return new WaitUntil(() => keyEvent.isKey || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1));
            if (Input.GetMouseButtonDown(0))
            {
                value = KeyCode.Mouse0;
                break;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                value = KeyCode.Mouse1;
                break;
            }
            else if (keyEvent.keyCode != KeyCode.None)
            {
                value = keyEvent.keyCode;
                break;
            }
            else
            {
                continue;
            }
        }
        SaveOneKey(type, value);
        KeyMappingEnd();
        _keyMapping = false;
    }

    private void OnGUI()
    {
        keyEvent = Event.current;
    }

    public void SaveOneKey(InputType keyType, KeyCode value)
    {
        Debug.Log($"KeyType : {keyType.ToString()}, KeyCode : {value.ToString()}");
        if (_saveKeys.ContainsKey(keyType))
            _saveKeys[keyType] = value;
        else
            _saveKeys.Add(keyType, value);
    }

    [ContextMenu("����!!")]
    public void SaveAllKey()
    {
        for (int i = 0; i < (int)InputType.Size; i++)
        {
            _saveKeys.TryAdd((InputType)i, defaultKeys[i]);
        }
        string path = Application.dataPath + "/Save/KeyData.json";
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(_saveKeys);
        Debug.Log(json);
        File.WriteAllText(path, json);
    }

    [ContextMenu("Ű ����")]
    public void ResetKey()
    {
        _saveKeys.Clear();
        SaveAllKey();
    }
}
