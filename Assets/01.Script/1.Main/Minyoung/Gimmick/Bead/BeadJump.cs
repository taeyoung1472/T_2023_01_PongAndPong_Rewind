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
            PlayerJump playerJump = other.GetComponent<PlayerJump>();
            playerJump.CurJumpCount = 0;
            Destroy(gameObject);
        }
    }
}
