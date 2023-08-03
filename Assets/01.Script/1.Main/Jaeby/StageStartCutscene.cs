using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StageStartCutscene : MonoBehaviour
{
    PlayableDirector _director = null;

    private void Start()
    {
        _director = GetComponent<PlayableDirector>();
        if (_director.playableAsset != null)
            _director.Play();
    }

    public void StopGameSystem()
    {

    }

    public void StartGameSystem()
    {

    }
}
