using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Playables;

public class DirectorContainer : MonoBehaviour
{
    [SerializeField]
    private List<PlayableDirector> _hiddenStageDirectors = new List<PlayableDirector>();

    public void PlayHiddenStage(int index)
    {
        _hiddenStageDirectors[index].Play();
    }
}
