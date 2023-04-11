using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeadJump : GimmickObject
{
    public override void Init()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.GetPlayerAction<PlayerJump>().MoreJump(0);
            Destroy(gameObject);
        }
    }
}
