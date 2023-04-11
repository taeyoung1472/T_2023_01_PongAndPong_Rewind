using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeadJump : GimmickObject
{
    
    public override void Init()
    {
    }
    public override void InitOnPlay()
    {
        
    }
    public override void InitOnRewind()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.GetPlayerAction<PlayerJump>().MoreJump(0);
            gameObject.SetActive(false);
        }
    }
}
