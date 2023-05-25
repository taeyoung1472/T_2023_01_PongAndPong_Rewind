using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatCategory : MonoBehaviour
{
    [SerializeField] private ChatVisual chatTemplate;
    [SerializeField] private Transform parent;
    public void Set(ChatDataSO data)
    {
        int index = 0;
        foreach (var dt in data.chatDatas)
        {
            ChatVisual visual = Instantiate(chatTemplate, parent);
            visual.Set(dt, data, index);
            visual.gameObject.SetActive(true);

            if (index == data.curChatIndex)
            {
                break;
            }

            index++;
        }
        gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }
}
