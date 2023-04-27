using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabableObject : MonoBehaviour
{
    [Header("벽 위치, 플레이어 기준에서 왼쪽 오른쪽만 구별할 수 있게만 하면 됨")]
    [SerializeField]
    private Transform _wallPosition = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Player>().GetPlayerAction<PlayerWallGrab>().WallEnter(gameObject, _wallPosition.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
    }
}
