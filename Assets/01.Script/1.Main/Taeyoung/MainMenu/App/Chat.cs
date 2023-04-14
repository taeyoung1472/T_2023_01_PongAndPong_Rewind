using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public void Start()
    {
        SetChatBtn();
    }

    private void SetChatBtn()
    {
        foreach (ChatTarget target in Enum.GetValues(typeof(ChatTarget)))
        {
            ChatBtn btn = Instantiate(chatBtnTemplate, chatBtnParent);
            btn.Set(chatDB.GetChatData(target), this);
            btn.gameObject.SetActive(true);
        }
    }

    public void ChangeTarget(ChatDataSO data)
    {
        targetName.SetText(data.myInfo.ToString());
        targetProfileName.SetText(data.myInfo.ToString());
        targetProfile.sprite = data.profile;
    }
}
