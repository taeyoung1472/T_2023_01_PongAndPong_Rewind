using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolable : PoolAbleObject
{
    ParticleSystem _particleSystem = null;

    public override void Init_Pop()
    {
        if (_particleSystem == null)
            _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem.Play();
    }

    public override void Init_Push()
    {
    }

    private void LateUpdate()
    {
        if(_particleSystem.particleCount == 0)
            PoolManager.Push(poolType, gameObject);
    }
}
