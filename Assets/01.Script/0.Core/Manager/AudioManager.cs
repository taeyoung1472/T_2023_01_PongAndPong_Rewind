using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioMixer mixer;
    private static AudioDataBase dataBase;
    public static AudioMixer Mixer
    {
        get
        {
            if (mixer == null)
                mixer = ResourceManager.Load<AudioMixer>("Core/Audio/Mixer");
            return mixer;
        }
    }
    public static AudioDataBase DataBase
    {
        get
        {
            if (dataBase == null)
            {
                dataBase = ResourceManager.Load<AudioDataBase>("Core/Data/AudioDB");
                dataBase.GenDic();
            }
            return dataBase;
        }
    }

    public static void PlayAudio(SoundType type, float pitch = 1f, float volume = 1f)
    {
        PlayAudio(DataBase.GetAudio(type), pitch, volume);
    }
    public static void PlayAudioRandPitch(SoundType type, float pitch = 1f, float randValue = 0.1f, float volume = 1f)
    {
        PlayAudioRandPitch(DataBase.GetAudio(type), pitch, randValue, volume);
    }

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
