using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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


    private CircularBuffer<bool> trackMotionTrail;
    private CircularBuffer<TrackMotionTrailData> trackMotionTrailData;

    public struct TrackMotionTrailData
    {
        public Mesh mesh;
        public MeshFilter meshFilter;
        public Vector3 position;
        public Quaternion rotation;
    }

    private Player player;
    protected override void Init()
    {
        player = GetComponent<Player>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        base.Init();
        trackMotionTrail = new CircularBuffer<bool>();
        trackMotionTrailData = new CircularBuffer<TrackMotionTrailData>();

        InitializeParticles(particleSettings);
    }

    protected override void InitOnPlay()
    {
        player.playerTrail.IsRewindMotionTrail = false;
        trackMotionTrail.InitBuffer();
        trackMotionTrailData.InitBuffer();
        InitBuffer();

        foreach (var item in enableList)
        {
            item.enabled = true;
        }

    }

    protected override void InitOnRewind()
    {
        player.playerTrail.IsRewindMotionTrail = true;
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
        trackMotionTrail.WriteLastValue(player.playerTrail.IsMotionTrail);
        
    }
    public void RestoreDashTrail(float seconds)
    {
        if (seconds + player.playerTrail.trailData.trailSpawnTime >= TimerManager.Instance.RewindingTime)
            return;

        player.playerTrail.IsMotionTrail =
            trackMotionTrail.ReadFromBuffer(seconds + player.playerTrail.trailData.trailSpawnTime);
        
    }
    private void Update()
    {
        
    }
}
