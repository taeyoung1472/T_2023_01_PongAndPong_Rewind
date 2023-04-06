using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "DB/Sound")]
public class AudioDataBase : ScriptableObject
{
    public SoundData[] soundDataArr;
    private Dictionary<SoundType, AudioClip[]> soundDic = new();

    public void GenDic()
    {
        for (int i = 0; i < soundDataArr.Length; i++)
        {
            soundDic.Add(soundDataArr[i].type, soundDataArr[i].clips);
        }
    }

    public AudioClip GetAudio(SoundType type)
    {
        return soundDic[type][Random.Range(0, soundDic[type].Length)];
    }

    [System.Serializable]
    public class SoundData
    {
        public SoundType type;
        public AudioClip[] clips;
    }
}

public enum SoundType
{
    Footstep,
    DashAir,
    DashGround,
    Attack,
    Jump,
    OnGrounded
}