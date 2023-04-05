using UnityEngine;

public class GenericRewind : RewindAbstract
{
    [SerializeField] bool trackPositionRotation;
    [SerializeField] bool trackVelocity;
    [SerializeField] bool trackAnimator;
    [SerializeField] bool trackAudio;
    [SerializeField] bool trackParticles;

    [Tooltip("파티클 추적을 선택한 경우에만 파티클 설정 채우기")]
    [SerializeField] ParticlesSetting particleSettings;

    private Vector3 originalPos;
    private Quaternion originalRot;

    protected override void InitOnPlay()
    {
        Debug.Log(transform.position + name + "3");
        InitBuffer();
        Debug.Log(transform.position + name + "4");
    }

    protected override void InitOnRewind()
    {

    }

    protected override void RestartObj()
    {
        Debug.Log("오브젝트 리셋");
        transform.SetPositionAndRotation(originalPos, originalRot);
        Debug.Log(transform.position + name + "??");
    }

    protected override void Rewind(float seconds)
    {

        if (trackPositionRotation)
            RestorePositionAndRotation(seconds);
        if (trackVelocity)
            RestoreVelocity(seconds);
        if (trackAnimator)
            RestoreAnimator(seconds);
        if (trackParticles)
            RestoreParticles(seconds);
        if(trackAudio)
            RestoreAudio(seconds);
    }

    protected override void Track()
    {
        if (trackPositionRotation)
            TrackPositionAndRotation();
        if (trackVelocity)
            TrackVelocity();
        if (trackAnimator)
            TrackAnimator();
        if (trackParticles)
            TrackParticles();
        if (trackAudio)
            TrackAudio();
    }



    protected override void Awake()
    {
        base.Awake();
        Debug.Log(transform.position + name + "5");
        InitializeParticles(particleSettings);
        originalPos = transform.position;
        originalRot = transform.rotation;
    }

}

