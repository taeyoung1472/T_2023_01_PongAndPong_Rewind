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

    private Dictionary<InputType, KeyCode> _saveKeys = new Dictionary<InputType, KeyCode>();

    private void Start()
    {
        UIInit();
    }

    private void UIInit()
    {
        for(int i = 0; i < (int)InputType.Size; i++)
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
        while(true)
        {
            yield return new WaitUntil(() => keyEvent.isKey);
            if (keyEvent.keyCode == KeyCode.None)
                continue;
            else
                break;
        }
        Debug.Log($"{type.ToString()}");
        SaveOneKey(type, keyEvent.keyCode);
        KeyMappingEnd();
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
        for (int i = 0; i < (int)InputType.Size; i++)
        {
            _saveKeys.TryAdd((InputType)i, defaultKeys[i]);
        }
        string path = Application.dataPath + "/Save/KeyData.json";
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(_saveKeys);
        Debug.Log(json);
        File.WriteAllText(path, json);
    }

    [ContextMenu("키 리셋")]
    public void ResetKey()
    {
        _saveKeys.Clear();
        SaveAllKey();
    }
}
