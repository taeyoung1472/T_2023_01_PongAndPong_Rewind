using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeadDash : GimmickObject
{
    public override void Init()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.GetPlayerAction<PlayerDash>(PlayerActionType.Dash).MoreDash(0);
            gameObject.SetActive(false);
        }
    }
}
