using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParticleSpawn : MonoSingleTon<TestParticleSpawn>
{
    public Transform playerPos;
    public ParticleRewind particleRewind;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameObject obj = PoolManager.Pop(PoolType.TestEffect);
            obj.transform.position = playerPos.position;
        }
    }
}
