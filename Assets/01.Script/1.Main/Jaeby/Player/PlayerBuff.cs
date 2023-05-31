using UnityEngine;

public class PlayerBuff : MonoBehaviour, IPlayerEnableResetable, IPlayerDisableResetable
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

    public void EnableReset()
    {
        _buff = 0;
    }

    public void DisableReset()
    {
        _buff = 0;
    }
}
