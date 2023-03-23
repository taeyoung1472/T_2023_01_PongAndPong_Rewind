using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class RewindAbstract : MonoBehaviour
{
    //트래킹은 한마디로 추적한다는 거임. 그니깐 순행 시간 추적해서 오브젝트 저장한다는 뜻
    public bool IsTracking { get; set; } = false;

    protected Rigidbody body;
    protected Rigidbody2D body2;
    protected Animator animator;
    protected AudioSource audioSource;


    protected void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        if (RewindManager.Instance != null)
        {
            body = GetComponent<Rigidbody>();
            body2 = GetComponent<Rigidbody2D>();
            if(animator == null)
                animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

            //IsTracking = true;
        }
        else
        {
            Debug.LogError("씬에서 TimeManager 스크립트를 찾을 수 없음. 시간 추적을 시작할 수 없습니다. 씬에 넣는 거 까먹었니 빠가야??");
        }

        trackedPositionsAndRotation = new CircularBuffer<PositionAndRotationValues>();
        trackedVelocities = new CircularBuffer<Vector3>();
        trackedAnimationTimes = new List<CircularBuffer<AnimationValues>>();
        if (animator != null)
        {
            for (int i = 0; i < animator.layerCount; i++)
                trackedAnimationTimes.Add(new CircularBuffer<AnimationValues>());
        }
        trackedAudioTimes = new CircularBuffer<AudioTrackedData>();
    }
    public void InitBuffer()
    {
        //Debug.Log("버퍼 초기화");
        trackedPositionsAndRotation.InitBuffer();
        trackedVelocities.InitBuffer();
        trackedAudioTimes.InitBuffer();

        if (animator != null)
        {
            for (int i = 0; i < animator.layerCount; i++)
            {
                trackedAnimationTimes[i].InitBuffer();
                //trackedAnimationTimes.Add(new CircularBuffer<AnimationValues>());
            }
        }

        IsTracking = true;
    }

    protected void FixedUpdate()
    {
        if (IsTracking)
            Track();
    }

    #region PositionRotation

    CircularBuffer<PositionAndRotationValues> trackedPositionsAndRotation;
    public struct PositionAndRotationValues
    {
        public Vector3 position;
        public Quaternion rotation;
        //public Vector2 inputDir;
    }

    /// <summary>
    /// 개체 위치 및 회전을 추적하려면 Track()에서 이 메서드를 호출
    /// </summary>
    protected void TrackPositionAndRotation()
    {
        PositionAndRotationValues valuesToWrite;
        valuesToWrite.position = transform.position;
        valuesToWrite.rotation = transform.rotation;

        trackedPositionsAndRotation.WriteLastValue(valuesToWrite);
    }
    /// <summary>
    /// GetSnapshotFromSavedValues()에서 이 메서드를 호출하여 위치 및 회전을 되돌림
    /// </summary>
    protected void RestorePositionAndRotation(float seconds)
    {
        PositionAndRotationValues valuesToRead = trackedPositionsAndRotation.ReadFromBuffer(seconds);
        transform.SetPositionAndRotation(valuesToRead.position, valuesToRead.rotation);
        //transform.position = Vector3.Slerp(transform.position, valuesToRead.position, seconds);
        //transform.rotation = Quaternion.Lerp(transform.rotation, valuesToRead.rotation, seconds);
    }
    #endregion

    #region Velocity
    CircularBuffer<Vector3> trackedVelocities;
    /// <summary>
    /// Rigidbody의 속도를 추적하려면 Track()에서 이 메서드를 호출하삼
    /// </summary>
    protected void TrackVelocity()
    {

        if (body != null)
        {
            trackedVelocities.WriteLastValue(body.velocity);            
        }
        else if (body2!=null)
        {
            trackedVelocities.WriteLastValue(body2.velocity);
        }
        else
        {
            Debug.LogError("TrackVelocity()가 호출되는 동안 개체에서 Rigidbody를 찾을 수 없다고!!!");
        }
    }
    /// <summary>
    /// Rigidbody의 속도로 GetSnapshotFromSavedValues()에서 이 메서드를 호출
    /// </summary>
    protected void RestoreVelocity(float seconds)
    {   
        if(body!=null)
        {
            body.velocity = trackedVelocities.ReadFromBuffer(seconds);
        }
        else
        {
            body2.velocity = trackedVelocities.ReadFromBuffer(seconds);
        }
    }
    #endregion

    #region Animator
    List<CircularBuffer<AnimationValues>> trackedAnimationTimes;         //모든 애니메이터 레이어를 추척
    public struct AnimationValues
    {
        public float animationStateTime;
        public int animationHash;
        public float animationTransition;
        public int animatorTransitionType;
    }
    /// <summary>
    /// 애니메이터 상태를 추적하려면 Track()에서 이 메서드를 호출해라
    /// </summary>
    protected void TrackAnimator()
    {
        if(animator == null)
        {
            Debug.LogError("TrackAnimator()가 호출되는 동안 개체에서 Animator를 찾을 수 없음");
            return;
        }

        animator.speed = 1;

        for (int i = 0; i < animator.layerCount; i++)
        {
            AnimatorStateInfo animatorInfo = animator.GetCurrentAnimatorStateInfo(i);
            AnimatorTransitionInfo animatorTransitionInfo = animator.GetAnimatorTransitionInfo(i);
            //Debug.Log(animatorTransitionInfo.normalizedTime + " : normalizedTime" + animatorTransitionInfo.duration +"<>"+ animatorTransitionInfo.durationUnit);


            AnimationValues valuesToWrite;
            valuesToWrite.animationStateTime = animatorInfo.normalizedTime;
            valuesToWrite.animationHash = animatorInfo.shortNameHash;

            valuesToWrite.animationTransition= animatorTransitionInfo.duration;
            valuesToWrite.animatorTransitionType = (int)animatorTransitionInfo.durationUnit;

            trackedAnimationTimes[i].WriteLastValue(valuesToWrite);
        }         
    }
    /// <summary>
    /// GetSnapshotFromSavedValues()에서 이 메서드를 호출하여 애니메이터 상태를 되돌림
    /// </summary>
    protected void RestoreAnimator(float seconds)
    {
        animator.speed = 0;
        //bool isFix = false;
        
        for(int i=0;i<animator.layerCount;i++)
        {
            AnimationValues readValues = trackedAnimationTimes[i].ReadFromBuffer(seconds);
            animator.Play(readValues.animationHash, i, readValues.animationStateTime);
            //if (readValues.animatorTransitionType == 0 && !isFix)
            //{
            //    Debug.Log("Fixed");
            //    isFix = true;
            //    animator.CrossFade(readValues.animationHash, readValues.animationTransition, i);
            //}
            //else
            //{
            //    Debug.Log("Not Fixed");
            //    isFix = false;
            //    animator.Play(readValues.animationHash, i, readValues.animationStateTime);
            //}
        }         
    }
    #endregion

    #region Audio
    CircularBuffer<AudioTrackedData> trackedAudioTimes;
    public struct AudioTrackedData
    {
        public float time;
        public bool isPlaying;
        public bool isEnabled;
    }
    /// <summary>
    /// 오디오를 추적하려면 Track()에서 이 메소드를 호출
    /// </summary>
    protected void TrackAudio()
    {
        if(audioSource==null)
        {
            Debug.LogError("TrackAudio()가 호출되는 동안 개체에서 AudioSource를 찾을 수 없음");
            return;
        }

        audioSource.volume = 1;
        AudioTrackedData dataToWrite;
        dataToWrite.time = audioSource.time;
        dataToWrite.isEnabled = audioSource.enabled;
        dataToWrite.isPlaying = audioSource.isPlaying;

        trackedAudioTimes.WriteLastValue(dataToWrite);      
    }
    /// <summary>
    /// GetSnapshotFromSavedValues()에서 이 메서드를 호출하여 오디오를 되돌림
    /// </summary>
    protected void RestoreAudio(float seconds)
    {
        AudioTrackedData readValues = trackedAudioTimes.ReadFromBuffer(seconds);
        audioSource.enabled = readValues.isEnabled;
        if(readValues.isPlaying)
        {
            audioSource.time = readValues.time;
            audioSource.volume = 0;

            if (!audioSource.isPlaying)
            {  
                audioSource.Play();
            }
        }
        else if(audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
    #endregion

    #region Particles
    //private float particleTimeLimiter;
    //private float particleResetTimeTo;
    List<CircularBuffer<ParticleTrackedData>> trackedParticleTimes = new List<CircularBuffer<ParticleTrackedData>>();
    public struct ParticleTrackedData
    {
        public bool isActive;
        public float particleTime;
    }


    private List<ParticleData> particleSystemsData;

    /// <summary>
    /// 파티클 시스템 및 해당 인에이블러는 파티클 시스템 게임 객체가 활성화 또는 비활성화되었는지 추적하기 위한 것임
    /// 퐈띠꿀
    /// </summary>
    [Serializable]
    public struct ParticleData
    {
        
        public ParticleSystem particleSystem;
        [Tooltip("파티클 시스템 게임 개체 활성 상태를 추적하기 위한 입자 시스템 인에이블러")]
        public GameObject particleSystemEnabler;
    }
    /// <summary>
    /// 사용자 지정 변수 추적에서 파티클을 설정하기 위한 입자 설정
    /// </summary>
    [Serializable]
    public struct ParticlesSetting
    {
        //[Tooltip("오래 지속되는 파티클 시스템의 경우 시간 추적 제한기를 설정하여 성능을 대폭 향상 ")]
        //public float particleLimiter;
        //[Tooltip("파티클 추적 한계에 도달한 후 추적이 반환되어야 하는 초를 정의하는 변수" +
        //    " 이 변수를 사용하여 더 나은 결과를 얻으면 추적 재설정이 크게 눈에 띄지 않음.")]
        //public float particleResetTo;
        public List<ParticleData> particlesData;
    }

    /// <summary>
    /// 파티클 되감기 구현을 사용할 때 이 함수를 먼저 사용하삼.
    /// </summary>
    /// <param name="particleDataList">추적할 입자를 정의하는 데이터</param>
    protected void InitializeParticles(ParticlesSetting particleSettings)
    {
        if(particleSettings.particlesData.Any(x=>x.particleSystemEnabler==null||x.particleSystem==null))
        {
            Debug.LogError("초기화된 파티클 시스템에 데이터가 없음. 일부 값에 대해 파티클 시스템 또는 파티클 시스템 인에이블러가 채워지지 않음");
        }
        particleSystemsData = particleSettings.particlesData;
        //particleTimeLimiter = particleSettings.particleLimiter;
        //particleResetTimeTo = particleSettings.particleResetTo;
        particleSystemsData.ForEach(x => trackedParticleTimes.Add(new CircularBuffer<ParticleTrackedData>()));

        foreach (CircularBuffer<ParticleTrackedData> i in trackedParticleTimes)
        {
            i.InitBuffer();
            ParticleTrackedData trackedData;
            trackedData.particleTime = 0;
            trackedData.isActive = false;
            i.WriteLastValue(trackedData);
        }
    }
    /// <summary>
    /// 파티클을 추적하려면 Track()에서 이 메서드를 호출(InitializeParticles()를 미리 호출해야 함).
    /// </summary>
    protected void TrackParticles()
    {
        if(particleSystemsData==null)
        {
            Debug.LogError("파티클이 초기화되지 않았음 추적이 시작되기 전에 InitializeParticles()를 호출해야함.");
            return;
        }
        if(particleSystemsData.Count==0)
            Debug.LogError("파티클 데이터가 채워지지 않았음 Unity 에디터에서 파티클 데이터 채우기");

        try
        {
            for (int i = 0; i < particleSystemsData.Count; i++)
            {
                if (particleSystemsData[i].particleSystem.isPaused)
                    particleSystemsData[i].particleSystem.Play();

                ParticleTrackedData lastValue = trackedParticleTimes[i].ReadLastValue();
                float addTime = lastValue.particleTime + Time.fixedDeltaTime;

                ParticleTrackedData particleData;
                particleData.isActive = particleSystemsData[i].particleSystemEnabler.activeInHierarchy;

                if ((!lastValue.isActive) && (particleData.isActive))
                {
                    particleData.particleTime = 0;

                }
                else if (!particleData.isActive)
                {

                    particleData.particleTime = 0;
                }
                else
                {

                    particleData.particleTime = addTime;
                    //Debug.Log(particleData.particleTime+ " : " + addTime);
                }
                trackedParticleTimes[i].WriteLastValue(particleData);
            }
        }
        catch
        {     
            Debug.LogError("파티클 데이터가 제대로 채워지지 않았음" +
                " 각 요소에 대해 파티클 시스템 및 파티클 시스템 인에이블러 필드를 모두 채워");
        }

    }
    /// <summary>
    /// GetSnapshotFromSavedValues()에서 이 메서드를 입자로 호출
    /// </summary>
    protected void RestoreParticles(float seconds)
    {
        for (int i = 0; i < particleSystemsData.Count; i++)
        {
            GameObject particleEnabler = particleSystemsData[i].particleSystemEnabler;


            ParticleTrackedData particleTracked = trackedParticleTimes[i].ReadFromBuffer(seconds);

            if (particleTracked.isActive)
            {

                if (!particleEnabler.activeSelf)
                    particleEnabler.SetActive(true);

                //Debug.Log("alkdfjklasf: "+particleTracked.particleTime);
                particleSystemsData[i].particleSystem.Simulate(particleTracked.particleTime, true, true, true);
            }
            else
            {
                if (particleEnabler.activeSelf)
                    particleEnabler.SetActive(false);
            }
        }
    }
    #endregion

   
    private void OnTrackingChange(bool val)
    {
        IsTracking = val;
    }
    protected void OnEnable()
    {
        RewindManager.Instance.RewindTimeCall += Rewind;
        RewindManager.Instance.TrackingStateCall += OnTrackingChange;
        RewindManager.Instance.InitPlay += InitOnPlay;
        RewindManager.Instance.InitRewind += InitOnRewind;
    }
    protected void OnDisable()
    {
        if (RewindManager.Instance != null)
        {
            RewindManager.Instance.RewindTimeCall -= Rewind;
            RewindManager.Instance.TrackingStateCall -= OnTrackingChange;
            RewindManager.Instance.InitPlay -= InitOnPlay;
            RewindManager.Instance.InitRewind -= InitOnRewind;
        }
        
    }

    /// <summary>
    /// 모든 추적이 채워지는 기본 함수, 여기에서 특정 객체에 대해 추적할 항목을 선택할 수 있음
    /// </summary>
    protected abstract void Track();


    /// <summary>
    /// 모든 되감기가 채워지는 주요 함수, 여기에서 특정 개체에 대해 되감기를 선택할 수 있음.
    /// </summary>
    /// <param name="seconds">되감기를 원하는 초 수를 정의하는 매개변수</param>
    protected abstract void Rewind(float seconds);
    /// <summary>
    /// 되감기가 끝나거나 처음 시작할 때 호출 ( =순행 시간의 맨 처음에 실행됨)
    /// </summary>
    protected abstract void InitOnPlay();
    /// <summary>
    /// 되감기가 실행될 때 맨 처음 한 번 실행되는 함수
    /// </summary>
    protected abstract void InitOnRewind();

}      