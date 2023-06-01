using UnityEngine;

public class ParticlePoolable : PoolAbleObject
{
    ParticleSystem _particleSystem = null;
    ParticleRewind _particleRewind = null;
    ParticleRewind.ParticleData setting;
    bool isfirst = false;
    float time;

    public override void Init_Pop()
    {
        time = Time.time;
        if (_particleRewind == null || _particleSystem == null)
        {
            _particleSystem = GetComponent<ParticleSystem>();
            //_particleRewind = TestParticleSpawn.Instance.particleRewind;
        }

        //if (isfirst == false)
        //{
        //    setting.particleSystem = _particleSystem;
        //    setting.particleSystemEnabler = gameObject;
        //    _particleRewind.particleSettings.particlesData.Add(setting);
        //    _particleRewind.InitParticle(_particleRewind.particleSettings);
        //    isfirst = true;
        //}
        _particleSystem.Play();
    }

    public override void Init_Push()
    {
        float t = Mathf.Abs(time - Time.time);
        time = t;
        if (ParticleRewindManager.Instance && !RewindManager.Instance.IsBeingRewinded)
        {
            ParticleRewindManager.Instance.TrackParticles(
                TimerManager.Instance.CurrentTimer, time, PoolType, transform.position, transform.rotation, _particleSystem.main);
        }

        if (!isfirst)
        {
            isfirst = true;
        }
    }

    private void LateUpdate()
    {
        if(_particleSystem.particleCount == 0)
            PoolManager.Push(poolType, gameObject);
    }
}
