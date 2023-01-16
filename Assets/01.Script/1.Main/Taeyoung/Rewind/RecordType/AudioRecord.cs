using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioRecord : RecordObject
{

    public static AudioRecord Instance { get; private set; }

    int curIndex;

    List<List<AudioData>> audioDataList;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        Register();
    }

    public void PlayAudio(AudioClip clip, float pitch = 1f, float volume = 1f)
    {
        AudioPoolObject obj = PoolManager.Pop(PoolType.Sound).GetComponent<AudioPoolObject>();
        obj.Play(clip, pitch, volume);
    }
    public void PlayAudioRandPitch(AudioClip clip, float pitch = 1f, float randValue = 0.1f, float volume = 1f)
    {
        AudioPoolObject obj = PoolManager.Pop(PoolType.Sound).GetComponent<AudioPoolObject>();
        obj.Play(clip, pitch + Random.Range(-randValue, randValue), volume);
    }

    public override void ApplyData(int index, int nextIndexDiff)
    {

    }

    public override void DeRegister()
    {

    }

    public override void InitOnPlay()
    {

    }

    public override void InitOnRewind()
    {

    }

    public override void Recorde(int index)
    {
        curIndex = index;
    }

    public override void Register()
    {
        GenerateList<List<AudioData>>(ref audioDataList, new());
    }

    [Serializable]
    public struct AudioData
    {
        public AudioClip clip;
        public float pitch;
        public float volume;
    }
}
