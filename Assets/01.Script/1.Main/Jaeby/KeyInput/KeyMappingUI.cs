using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyMappingUI : MonoBehaviour
{

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
    private KeyInputUI _keyMappingPrefab = null;

    Event keyEvent;

    private bool _keyMapping = false;

    private Dictionary<InputType, KeyCode> _saveKeys = new();
    private Dictionary<InputType, KeyInputUI> keyUI = new();

    private void Start()
    {
        KeyManager.LoadKey();
        UIInit();
    }

    private void UIInit()
    {
        for (int i = 0; i < (int)InputType.Size; i++)
        {
            KeyInputUI keyUIPrefab = Instantiate(_keyMappingPrefab, _keyMappingPanel.transform);
            ButtonSet(keyUIPrefab.GetComponent<Button>(), i);
            keyUI.Add((InputType)i, keyUIPrefab);
            keyUIPrefab.DisplayData((InputType)i, KeyManager.keys[(InputType)i]);
        }
    }

    public void ButtonSet(Button button, int index)
    {
        button.onClick.AddListener(() => { KeyMappingStart((InputType)index); });
    }

    public void UIOn()
    {
        for (int i = 0; i < (int)InputType.Size; i++)
        {
            keyUI[(InputType)i].DisplayData((InputType)i, KeyManager.keys[(InputType)i]);
        }
        _mainPanel.SetActive(true);
        _startButton.SetActive(false);
    }

    public void UIOff()
    {
        _saveKeys.Clear();
        _mainPanel.SetActive(false);
        _startButton.SetActive(true);
    }

    public void KeyMappingStart(InputType type)
    {
        if (_keyMapping)
            return;
        _mappingStartUI.SetActive(true);
        _mappingStartUI.GetComponentInChildren<TextMeshProUGUI>().SetText($"원하는 버튼을\n준내게 누르세요\n<버튼 : {type.ToString()}>");
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
        keyUI[type].DisplayData(type, value);
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

    [ContextMenu("저장!!")]
    public void SaveAllKey()
    {
        KeyManager.ChangeKeySetting(_saveKeys);
        Debug.Log(_saveKeys.Count);
        Debug.Log(KeyManager.keys.Count);

        string path = Application.dataPath + "/Save/KeyData.json";
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(KeyManager.keys);
        Debug.Log(json);
        File.WriteAllText(path, json);
    }
}
