using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    [Header("[Data]")]
    [SerializeField] private ChatDB chatDB;

    [Header("[UI]")]
    [SerializeField] private TextMeshProUGUI targetName;
    [SerializeField] private TextMeshProUGUI targetProfileName;
    [SerializeField] private Image targetProfile;

    [Header("[ChatBtn]")]
    [SerializeField] private Transform chatBtnParent;
    [SerializeField] private ChatBtn chatBtnTemplate;
    private Dictionary<ChatTarget, ChatBtn> chatDic = new();
    private ChatBtn prevChatBtn;

    [Header("[ChatContent]")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private ChatCategory contentTemplate;
    private Dictionary<ChatTarget, ChatCategory> contentDic = new();
    private ChatCategory prevContentObject;

    public void Awake()
    {
        SetChatBtn();
        SetContent();
    }

    private void SetContent()
    {
        foreach (ChatTarget target in Enum.GetValues(typeof(ChatTarget)))
        {
            ChatCategory category = Instantiate(contentTemplate, contentParent);
            category.Set(chatDB.GetChatData(target));
            contentDic[target] = category;
        }
    }

    private void SetChatBtn()
    {
        foreach (ChatTarget target in Enum.GetValues(typeof(ChatTarget)))
        {
            ChatBtn btn = Instantiate(chatBtnTemplate, chatBtnParent);
            btn.Set(chatDB.GetChatData(target), this);
            btn.gameObject.SetActive(true);
            btn.OutFocus();
            chatDic[target] = btn;
        }
    }

    public void ChangeTarget(ChatDataSO data)
    {
        targetName.SetText(data.myInfo.ToString());
        targetProfileName.SetText(data.myInfo.ToString());
        targetProfile.sprite = data.profile;

        if (prevContentObject != null)
        {
            prevContentObject.gameObject.SetActive(false);
        }
        if (prevChatBtn != null)
        {
            prevChatBtn.OutFocus();
        }
        prevContentObject = contentDic[data.myInfo];
        prevContentObject.gameObject.SetActive(true);
        prevChatBtn = chatDic[data.myInfo];
        prevChatBtn.Focus();
    }
}
