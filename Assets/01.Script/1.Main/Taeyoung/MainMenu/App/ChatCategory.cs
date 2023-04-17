using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatCategory : MonoBehaviour
{
    [SerializeField] private ChatVisual chatTemplate;
    [SerializeField] private Transform parent;
    public void Set(ChatTarget target, ChatDataSO data)
    {
        foreach (var dt in data.chatDatas)
        {
            ChatVisual visual = Instantiate(chatTemplate, parent);
            visual.Set(dt);
            visual.gameObject.SetActive(true);
        }
    }
}
