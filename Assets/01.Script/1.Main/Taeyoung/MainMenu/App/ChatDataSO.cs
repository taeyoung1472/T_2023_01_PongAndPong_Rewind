using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/App/Chat")]
public class ChatDataSO : ScriptableObject
{
    public ChatTarget myInfo;
    public Sprite profile;
}

[Serializable]
public class ChatUnit
{

}

[Serializable]
public class ChatContent
{

}