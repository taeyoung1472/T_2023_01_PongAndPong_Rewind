using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabableObject : MonoBehaviour
{
    [Header("�� ��ġ, �÷��̾� ���ؿ��� ���� �����ʸ� ������ �� �ְԸ� �ϸ� ��")]
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
