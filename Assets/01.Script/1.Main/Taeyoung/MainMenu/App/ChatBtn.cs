using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBtn : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private Image profile;

    public void Set(ChatDataSO data, Chat chat)
    {
        nameTMP.SetText(data.myInfo.ToString());
        profile.sprite = data.profile;

        GetComponent<Button>().onClick.AddListener(() => chat.ChangeTarget(data));
    }
}
