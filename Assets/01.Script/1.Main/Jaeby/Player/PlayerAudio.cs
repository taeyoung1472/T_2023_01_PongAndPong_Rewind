using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField]
    private AudioClip _moveClip = null;

    public void MoveAudio()
    {
        AudioPoolObject audio = PoolManager.Pop(PoolType.Sound).GetComponent<AudioPoolObject>();
        audio.Play(_moveClip, Random.Range(0.5f, 1f));
    }
}
