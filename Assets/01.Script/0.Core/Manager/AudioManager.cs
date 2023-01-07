using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static void PlayAudio(AudioClip clip, float pitch = 1f, float volume = 1f)
    {
        AudioPoolObject obj = PoolManager.Pop(PoolType.Sound).GetComponent<AudioPoolObject>();
        obj.Play(clip, pitch, volume);
    }
    public static void PlayAudioRandPitch(AudioClip clip, float pitch = 1f, float randValue = 0.1f, float volume = 1f)
    {
        AudioPoolObject obj = PoolManager.Pop(PoolType.Sound).GetComponent<AudioPoolObject>();
        obj.Play(clip, pitch + Random.Range(-randValue, randValue), volume);
    }
}
