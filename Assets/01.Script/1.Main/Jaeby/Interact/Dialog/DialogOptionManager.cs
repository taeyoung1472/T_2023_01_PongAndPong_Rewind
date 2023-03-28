using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogOptionManager : MonoBehaviour
{
    [SerializeField]
    private Sprite _defaultIcon = null;
    public Sprite DefaultIcon => _defaultIcon;
    private Dictionary<string, UnityEvent> _optionEvents = new Dictionary<string, UnityEvent>();

    [SerializeField]
    private List<OptionEventData> optionEventDatas = new List<OptionEventData>();

    public UnityEvent GetEvent(string key)
    {
        Debug.Log($"key : {key}");
        if (_optionEvents.ContainsKey(key))
            return _optionEvents[key];
        else
            return null;
    }

    private void Awake()
    {
        for (int i = 0; i < optionEventDatas.Count; i++)
        {
            Debug.Log($"추가해용 key : {optionEventDatas[i].key}");
            _optionEvents.TryAdd(optionEventDatas[i].key, optionEventDatas[i].optionEvent);
        }
    }
}

[System.Serializable]
public struct OptionEventData
{
    public string key;
    public UnityEvent optionEvent;
}
