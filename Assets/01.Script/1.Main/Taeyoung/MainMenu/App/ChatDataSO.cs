using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/App/Chat")]
public class ChatDataSO : ScriptableObject
{
    public ChatTarget myInfo;
    public Sprite profile;

    public List<ChatUnit> chatDatas;
}

[Serializable]
public class ChatUnit
{
    public ChatContent[] chatContents;
}

[Serializable]
public class ChatContent
{
    public string text;
    public Sprite image;
    public bool isRead;
}