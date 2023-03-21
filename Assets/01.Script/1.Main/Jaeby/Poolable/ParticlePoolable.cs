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

    private void OnParticleSystemStopped()
    {
        PoolManager.Push(poolType, gameObject);
    }
}
