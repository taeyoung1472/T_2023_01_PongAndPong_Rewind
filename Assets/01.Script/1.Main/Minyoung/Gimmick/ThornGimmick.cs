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
            //�翱�̰����� �÷��̾���̿� �����Լ�
            //player.GetComponentInChildren<PlayerDie>().Die();
        }       
    }
}
