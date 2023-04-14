using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/App/ChatDB")]
public class ChatDB : ScriptableObject
{
    [SerializeField] private List<ChatDataSO> chatDatas;
    Dictionary<ChatTarget, ChatDataSO> chatDic = new();
    Dictionary<ChatTarget, ChatDataSO> ChatDic
    {
        get
        {
            if (chatDic.Count == 0)
            {
                foreach (var data in chatDatas)
                {
                    chatDic.Add(data.myInfo, data);
                }
            }
            return chatDic;
        } 
    }

    public ChatDataSO GetChatData(ChatTarget target)
    {
        return ChatDic[target];
    }
}

public enum ChatTarget
{
    이재엽,
    이승현,
    유진우,
    이태영,
    한민영
}