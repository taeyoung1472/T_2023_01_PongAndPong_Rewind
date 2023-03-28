using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornGimmick : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GimmickPlayerCol"))
        {
            Player player = other.GetComponentInParent<Player>();
            Debug.Log(player);
            //재엽이가만들 플레이어다이에 다이함수
            //player.GetComponentInChildren<PlayerDie>().Die();
        }       
    }
}
