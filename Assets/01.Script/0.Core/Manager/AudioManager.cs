using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioMixer mixer;
    private static AudioDataBase dataBase;
    private static Dictionary<string, AudioMixerGroup> mixerDic;

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
    public static Dictionary<string, AudioMixerGroup> MixerDic
    {
        get
        {
            if (mixerDic == null)
            {
                mixerDic = new();

                mixerDic.Add("", Mixer.FindMatchingGroups("SFX")[0]);
                mixerDic.Add("SFX", Mixer.FindMatchingGroups("SFX")[0]);
                mixerDic.Add("BGM", Mixer.FindMatchingGroups("BGM")[0]);
                mixerDic.Add("Another", Mixer.FindMatchingGroups("Another")[0]);
            }
            return mixerDic;
        }
    }

    public static void PlayAudio(SoundType type, float pitch = 1f, float volume = 1f, string outputName = "")
    {
        PlayAudio(DataBase.GetAudio(type), pitch, volume, outputName);
    }
    public static void PlayAudioRandPitch(SoundType type, float pitch = 1f, float randValue = 0.1f, float volume = 1f, string outputName = "")
    {
        PlayAudioRandPitch(DataBase.GetAudio(type), pitch, randValue, volume, outputName);
    }

    public static void PlayAudio(AudioClip clip, float pitch = 1f, float volume = 1f, string outputName = "")
    {
        AudioPoolObject obj = PoolManager.Pop(PoolType.Sound).GetComponent<AudioPoolObject>();
        obj.Play(clip, pitch, volume, MixerDic[outputName]);
    }
    public static void PlayAudioRandPitch(AudioClip clip, float pitch = 1f, float randValue = 0.1f, float volume = 1f, string outputName = "")
    {
        AudioPoolObject obj = PoolManager.Pop(PoolType.Sound).GetComponent<AudioPoolObject>();
        obj.Play(clip, pitch + Random.Range(-randValue, randValue), volume, MixerDic[outputName]);
    }
}
