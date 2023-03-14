using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRewindTest : RewindAbstract
{
    [SerializeField] private bool trackPositionRotation;
    [SerializeField] private bool trackVelocity;
    [SerializeField] private bool trackAnimator;
    [SerializeField] private bool trackAudio;
    [SerializeField] private bool trackParticles;

    [Tooltip("파티클 추적을 선택한 경우에만 파티클 설정 채우기")]
    [SerializeField] ParticlesSetting particleSettings;


    [SerializeField] private List<MonoBehaviour> enableList;

    [SerializeField] private CharacterController characterController;
    protected override void Init()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        base.Init();
        characterController = GetComponent<CharacterController>();

    }

    protected override void InitOnPlay()
    {
        foreach (var item in enableList)
        {
            item.enabled = true;
        }

        characterController.enabled = true;
    }

    protected override void InitOnRewind()
    {

        foreach (var item in enableList)
        {
            item.enabled = false;
        }

        characterController.enabled = false;
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
    private void Start()
    {
        InitializeParticles(particleSettings);
    }
}
