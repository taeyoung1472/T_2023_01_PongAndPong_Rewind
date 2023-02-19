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
        if (_optionEvents.ContainsKey(key))
            return _optionEvents[key];
        else
            return null;
    }

    private void OnValidate()
    {
        for (int i = 0; i < optionEventDatas.Count; i++)
        {
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
