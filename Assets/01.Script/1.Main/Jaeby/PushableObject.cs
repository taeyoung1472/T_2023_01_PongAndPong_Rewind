using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == false)
            return;
        other.GetComponent<Player>().GetPlayerAction<PlayerObjectPush>().PushStart(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") == false)
            return;
        other.GetComponent<Player>().GetPlayerAction<PlayerObjectPush>().PushEnd(gameObject);
    }
}
