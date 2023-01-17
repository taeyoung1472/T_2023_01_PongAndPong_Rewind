using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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
        pitch = -1;

        AudioPoolObject obj = PoolManager.Pop(PoolType.Sound).GetComponent<AudioPoolObject>();
        obj.Play(clip, pitch, volume);

        audioDataList[curIndex].Add(new AudioData(clip, pitch, volume, RewindManager.Instance.IsRewinding));
    }
    public void PlayAudioRandPitch(AudioClip clip, float pitch = 1f, float randValue = 0.1f, float volume = 1f)
    {
        AudioPoolObject obj = PoolManager.Pop(PoolType.Sound).GetComponent<AudioPoolObject>();
        obj.Play(clip, pitch + Random.Range(-randValue, randValue), volume);

        audioDataList[curIndex].Add(new AudioData(clip, pitch + Random.Range(-randValue, randValue), volume, RewindManager.Instance.IsRewinding));
    }

    public override void ApplyData(int index, int nextIndexDiff)
    {
        float targetPitch;
        bool isRewinding = nextIndexDiff > 0 ? true : false;

        Debug.Log($"{index} , {audioDataList[index].Count}");
        for (int i = 0; i < audioDataList[index].Count; ++i)
        {
            targetPitch = audioDataList[index][i].pitch;// * ((isRewinding ? -1 : 1) * (audioDataList[index][i].isRewinding ? -1 : 1));

            Debug.Log(targetPitch);

            AudioPoolObject obj = PoolManager.Pop(PoolType.Sound).GetComponent<AudioPoolObject>();
            obj.Play(audioDataList[index][i].clip, targetPitch, audioDataList[index][i].volume);
        }
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

        for (int i = 0; i < audioDataList.Count; i++)
        {
            audioDataList[i] = new();
        }

        RewindManager.Instance.RegistRecorder(this);
    }

    [Serializable]
    public struct AudioData
    {
        public AudioData(AudioClip clip, float pitch, float volume, bool isRewinding)
        {
            this.clip = clip;
            this.pitch = pitch;
            this.volume = volume;
            this.isRewinding = isRewinding;
        }

        public AudioClip clip;
        public float pitch;
        public float volume;
        public bool isRewinding;
    }
}
