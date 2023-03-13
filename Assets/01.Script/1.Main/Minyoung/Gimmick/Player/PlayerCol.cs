using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCol : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("RingGimmick"))
        {
            Debug.Log("¸µ");
            this.GetComponent<PlayerDash>().Dash();
        }
    }
}
