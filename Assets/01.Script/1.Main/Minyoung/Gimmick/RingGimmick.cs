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
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name);

            PlayerDash player = other.GetComponent<PlayerDash>();
            player.Dash();
        }
    }
}
