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
    // �� ����
    public Sprite communicationSprite = null;
    // ����
    public string content = "";
    // �̸�ŭ ��ٸ��� ���� ����
    public float nextContentTime = 1f;
    // true�� ��� �����ϰ� ����
    public bool isReset = false;
}