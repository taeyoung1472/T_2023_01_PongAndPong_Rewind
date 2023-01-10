using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoSingleTon<PlayerSpawner>
{
    [SerializeField] private Player player;
    [SerializeField] private Vector3 playerSpawnPos;

    public void Spawn()
    {
        Instantiate(player, playerSpawnPos, Quaternion.identity);
    }
}
