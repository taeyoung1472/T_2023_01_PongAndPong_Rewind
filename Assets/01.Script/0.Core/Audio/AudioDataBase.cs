using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public void OnValidate()
    {
        soundDataArr = soundDataArr.OrderBy(a => a.type).ToArray();
        for (int i = 0; i < soundDataArr.Length; i++)
        {
            soundDataArr[i].name = $"{(int)soundDataArr[i].type}_{soundDataArr[i].type}";
        }
    }

    [System.Serializable]
    public class SoundData
    {
        [HideInInspector] public string name;
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
    OnCollect = 602,
    OnGameEnd = 603,
    OnStageClear = 604,
    OnSceneChange = 605,

    OnNPCSpeak = 700,
    OnLabDoorOpen = 701,
    OnLabDoorClose = 702,
    OnOpenStageInfo = 703,
    OnCloseStageInfo = 704,
    OnNotifyOpen = 710,
    OnNotifyClose = 711,

    StartCutScene = 1000,
    SkipSound = 1001,
    OnMonitor = 1002,
}