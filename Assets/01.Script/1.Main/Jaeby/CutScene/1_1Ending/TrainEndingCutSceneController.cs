using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class TrainEndingCutSceneController : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector _cutScene = null;
    [SerializeField]
    private TrailableObject _trailableObj = null;
    [SerializeField]
    private StageCommunicationSO _comuData = null;
    [SerializeField]
    private ParticleSystem _particleSystem = null;

    private void Start()
    {
    }

    private void Dynamicbinding()
    {
        TimelineAsset ta = _cutScene.playableAsset as TimelineAsset;
        IEnumerable<TrackAsset> temp = ta.GetOutputTracks();
        foreach (TrackAsset track in temp)
        {
            if(track is CinemachineTrack)
            {
                Debug.Log("찾았다.");
                _cutScene.SetGenericBinding(track, Utility.SearchByClass<CinemachineBrain>());
            }
        }
    }

    [ContextMenu("테스트로 시작")]
    public void CutSceneStart()
    {
        //TimerManager.Instance.EndRewind();
        Dynamicbinding();
        _cutScene.Play();
    }

    public void TrailStart()
    {
        TrailManager.Instance.AddTrailObj(_trailableObj);
        _trailableObj.IsMotionTrail = true;
        _particleSystem.Play();
    }

    public void ComuStart()
    {
        StageCommunicationUI.Instance.CommunicationStart(_comuData);
    }

    public void GoNewLab()
    {
        SceneManager.LoadScene("NewLab");
    }
}
