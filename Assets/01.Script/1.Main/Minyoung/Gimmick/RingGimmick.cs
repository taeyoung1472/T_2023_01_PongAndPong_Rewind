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
            Debug.Log("디버그");
            Player player = other.GetComponent<Player>();
            //Debug.Log("대시 상태 " + player.PlayerActionCheck(PlayerActionType.Dash));
            player.PlayerActionExit(PlayerActionType.Dash); //대쉬를 강제종료
            player.GetPlayerAction<PlayerDash>().MoreDash(0);
            player.GetPlayerAction<PlayerDash>().Dash(player.PlayerRenderer.Forward);
        }
    }
}
