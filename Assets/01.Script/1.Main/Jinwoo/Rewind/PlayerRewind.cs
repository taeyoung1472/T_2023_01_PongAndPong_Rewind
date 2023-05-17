using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRewind : RewindAbstract
{
    [SerializeField] private bool trackPositionRotation;
    [SerializeField] private bool trackVelocity;
    [SerializeField] private bool trackAnimator;
    [SerializeField] private bool trackAudio;
    [SerializeField] private bool trackParticles;
    [SerializeField] private bool trackDashTrail;

    [Tooltip("파티클 추적을 선택한 경우에만 파티클 설정 채우기")]
    [SerializeField] ParticlesSetting particleSettings;


    [SerializeField] private List<MonoBehaviour> enableList;


    private Player player;
    protected override void Init()
    {
        player = GetComponent<Player>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        base.Init();

        InitializeParticles(particleSettings);
    }

    protected override void InitOnPlay()
    {
        InitBuffer();


        foreach (var item in enableList)
        {
            item.enabled = true;
        }

    }

    protected override void InitOnRewind()
    {

        foreach (var item in enableList)
        {
            item.enabled = false;
        }

    }

    protected override void RestartObj()
    {
        //player.playerTrail.DestroyTrailAll(false);
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
        if (trackDashTrail)
            RestoreDashTrail(seconds);
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
        if(trackDashTrail)
            TrackDashTrail();
    }

    public void TrackDashTrail()
    {

    }
    public void RestoreDashTrail(float seconds)
    {

    }
    private void Start()
    {
        
    }
}
