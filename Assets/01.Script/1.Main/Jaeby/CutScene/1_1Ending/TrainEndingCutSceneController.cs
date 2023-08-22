using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TrainEndingCutSceneController : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector _cutScene = null;
    [SerializeField]
    private TrailableObject _trailableObj = null;
    [SerializeField]
    private StageCommunicationSO _comuData = null;

    public void CutSceneStart()
    {
        _cutScene.Play();
    }

    public void TrailStart()
    {
        _trailableObj.TrailEnable();
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
