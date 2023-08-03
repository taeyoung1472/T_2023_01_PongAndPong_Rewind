using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/StageCommunication")]
public class StageCommunicationSO : ScriptableObject
{
    public List<CommunicationData> communicationDatas = new List<CommunicationData>();
    public int maxDataCount = 5;
    public float fadeTime = 0.3f;
    public float animationTime = 0.4f;
}

[System.Serializable]
public class CommunicationData
{
    // 얼굴 사진
    public Sprite communicationSprite = null;
    // 내용
    public string content = "";
    // 이만큼 기다리고 다음 내용
    public float nextContentTime = 1f;
    // true면 모두 삭제하고 내용
    public bool isReset = false;
}