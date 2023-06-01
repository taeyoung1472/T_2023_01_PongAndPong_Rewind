using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RewindAbstract;

public class ParticleRewindManager : MonoSingleTon<ParticleRewindManager>
{
    Dictionary<int, ParticleTrackedData> particleDic = new();
    List<ParticleRewindData> particleList = new();
    private int prevFixeTime;

    private void Awake()
    {
        RewindManager.Instance.InitPlay += Init;
    }
    
    void Init()
    {
        particleDic.Clear();
        particleList.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            GameObject obj = PoolManager.Pop(PoolType.TestEffect);
        }
        if (RewindManager.Instance.IsBeingRewinded)
        {
            RestoreParticles(TimerManager.Instance.RewindingTime - TimerManager.Instance.CurrentTimer);
        }
    }

    public void TrackParticles(float time, float particleTime, PoolType particleType, Vector3 pos, Quaternion rot , ParticleSystem.MainModule main)
    {
        int fixedTime = (int)(time * 10);

        ParticleTrackedData dt;
        dt.particleTime = particleTime;
        dt.particleType = particleType;
        dt.position = pos;
        dt.rot = rot;
        dt.main = main;

        if(!particleDic.ContainsKey(fixedTime))
        {
            particleDic.Add(fixedTime, dt);
        }
    }
    /// <summary>
    /// GetSnapshotFromSavedValues()에서 이 메서드를 입자로 호출
    /// </summary>
    protected void RestoreParticles(float seconds)
    {
        Debug.Log($"ㅇㅇ : {seconds}");
        int fixedTime = (int)(seconds * 10);

        for (int i = 0; i < particleList.Count; i++)
        {
            ParticleRewindData dt = particleList[i];

            if (seconds < dt.startTime)
            {
                PoolManager.Push(dt.poolAble.PoolType, dt.poolAble.gameObject);
                particleList.Remove(dt);
                i--;
                continue;
            }

            if (dt.isFirst)
            {
                dt.particle.Simulate(seconds - dt.startTime, true, true, false);
                dt.isFirst = false;
            }
            else
            {
                dt.particle.Simulate(seconds - dt.startTime, true, true, false);
            }
            particleList[i] = dt;

            Debug.Log($"ㅂㅂㅂ {seconds - dt.startTime}");
        }

        if(prevFixeTime == fixedTime)
        {
            return;
        }

        if (particleDic.ContainsKey(fixedTime))
        {
            ParticleTrackedData dt = particleDic[fixedTime];

            GameObject particle = PoolManager.Pop(dt.particleType);
            particle.transform.SetPositionAndRotation(dt.position, dt.rot);

            ParticleRewindData particleDt;
            particleDt.startTime = seconds - dt.particleTime;
            particleDt.particle = particle.GetComponent<ParticleSystem>();
            particleDt.poolAble = particle.GetComponent<PoolAbleObject>();
            particleDt.isFirst = true;

            particleList.Add(particleDt);
        }

        prevFixeTime = fixedTime;
    }

    public struct ParticleTrackedData
    {
        public ParticleSystem.MainModule main;
        public float particleTime;
        public PoolType particleType;
        public Vector3 position;
        public Quaternion rot;
    }

    public struct ParticleRewindData
    {
        public float startTime;
        public bool isFirst;
        public ParticleSystem particle;
        public PoolAbleObject poolAble;
    }
}
