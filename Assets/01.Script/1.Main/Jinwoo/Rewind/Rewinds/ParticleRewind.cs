using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRewind : RewindAbstract
{
    [SerializeField] private bool trackPositionRotation;
    [SerializeField] private bool trackVelocity;
    [SerializeField] private bool trackAnimator;
    [SerializeField] private bool trackAudio;
    [SerializeField] private bool trackParticles;

    [Tooltip("파티클 추적을 선택한 경우에만 파티클 설정 채우기")]
    public ParticlesSetting particleSettings;

    protected override void Init()
    {
        base.Init();

        //InitializeParticles(particleSettings);
    }
    public void InitParticle(ParticlesSetting setting)
    {
        InitializeParticles(setting);
        trackParticles = true;
    }
    protected override void InitOnPlay()
    {

    }

    protected override void InitOnRewind()
    {

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
        if (trackAudio)
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

    protected override void RestartObj()
    {

    }
}
