using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRewindTest : RewindAbstract
{
    [SerializeField] bool trackPositionRotation;
    [SerializeField] bool trackVelocity;
    [SerializeField] bool trackAnimator;
    [SerializeField] bool trackAudio;
    [SerializeField] bool trackParticles;

    [Tooltip("��ƼŬ ������ ������ ��쿡�� ��ƼŬ ���� ä���")]
    [SerializeField] ParticlesSetting particleSettings;

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
    private void Start()
    {
        InitializeParticles(particleSettings);
    }
}
