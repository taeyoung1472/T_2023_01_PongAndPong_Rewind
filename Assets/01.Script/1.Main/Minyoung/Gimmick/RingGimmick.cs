using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingGimmick : GimmickObject
{
    public override void Init()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isRewind)
        {
            return;
        }
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("�����");
            Player player = other.GetComponent<Player>();
            //Debug.Log("��� ���� " + player.PlayerActionCheck(PlayerActionType.Dash));
            player.PlayerActionExit(PlayerActionType.Dash); //�뽬�� ��������
            player.GetPlayerAction<PlayerDash>().MoreDash(0);
            player.GetPlayerAction<PlayerDash>().Dash(player.PlayerRenderer.Forward);
        }
    }
}
