using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff : MonoBehaviour
{
    private int _buff = 0;

    public void AddBuff(PlayerBuffType buffType)
    {
        _buff |= (int)buffType; // ������ �߰�
    }

    public void DeleteBuff(PlayerBuffType buffType)
    {
        _buff |= (int)buffType; // ������ �߰�
        _buff ^= (int)buffType; // �� �� 1�� ������ 0���� ��ȯ
    }

    public bool BuffCheck(PlayerBuffType buffType)
    {
        return (_buff & (int)buffType) > 0;
    }
}
