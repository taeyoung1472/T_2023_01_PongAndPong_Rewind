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
    OnObjectImpact = 0,

    Footstep = 100,

    DashAir = 200,
    DashGround = 201,

    Attack = 300,

    Jump = 400,
    OnGrounded = 401,

    OnJumpPad = 501,
    OnPortal = 502,
    OnChangeGravity = 503,  
    OnActiveButton = 511,
    OnDeActiveButton = 512,
    OnReplayBreak = 520,
    OnReplayDisplay = 521,

    OnDragingObject = 600,
    OnGameStart = 601,
}