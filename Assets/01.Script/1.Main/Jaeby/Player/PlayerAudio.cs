using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> _moveClips = new List<AudioClip>();

    public void MoveAudio()
    {
        AudioPoolObject audio = PoolManager.Pop(PoolType.Sound).GetComponent<AudioPoolObject>();
        audio.Play(_moveClips[Random.Range(0, _moveClips.Count)], Random.Range(0.5f, 1f));
    }
}
