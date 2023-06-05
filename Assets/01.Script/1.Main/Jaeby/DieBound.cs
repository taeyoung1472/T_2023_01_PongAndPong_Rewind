using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieBound : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Player>().playerHP.Die();
        }
    }
}
