using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioApplySound : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer = null;
    [SerializeField]
    private float _maxVol = 0f;
    [SerializeField]
    private float _minVol = 0f;

    public float MaxVol => _maxVol;
    public float MinVol => _minVol;

  //  private readonly string _masterSoundKey = "VOL_MASTER";
    private readonly string _bgmSoundKey = "VOL_BGM";
    private readonly string _sfxSoundKey = "VOL_SFX";

    //private readonly string _masterMixerKey = "Master";
    private readonly string _bgmMixerKey = "BGM";
    private readonly string _sfxMixerKey = "SFX";

  //  private float _masterSound = 0f;
    private float _bgmSound = 0f;
    private float _sfxSound = 0f;

    public float SfxSound
    {
        get => _sfxSound;
        set
        {
            _sfxSound = value;
            if (_sfxSound >= _maxVol)
            {
                _sfxSound = _maxVol;
            }
            else if (_sfxSound <= _minVol)
            {
                _sfxSound = -80f;
            }
            _audioMixer.SetFloat(_sfxMixerKey, _sfxSound);
        }
    }

    public float BGMSound
    {
        get => _bgmSound;
        set
        {
            _bgmSound = value;
            if (_bgmSound >= _maxVol)
            {
                _bgmSound = _maxVol;
            }
            else if (_bgmSound <= _minVol)
            {
                _bgmSound = -80f;
            }
            _audioMixer.SetFloat(_bgmMixerKey, _bgmSound);
        }
    }

    //public float MasterSound
    //{
    //    get => _masterSound;
    //    set
    //    {
    //        _masterSound = value;
    //        if (_masterSound >= _maxVol)
    //        {
    //            _masterSound = _maxVol;
    //        }
    //        else if (_masterSound <= _minVol)
    //        {
    //            _masterSound = -80f;
    //        }
    //        _audioMixer.SetFloat(_masterMixerKey, _masterSound);
    //    }
    //}

    private void Awake()
    {
        Init();
        Apply();
    }

    private void Init()
    {
        //_masterSound = PlayerPrefs.GetFloat(_masterSoundKey, (_maxVol + _minVol) * 0.5f);
        _bgmSound = PlayerPrefs.GetFloat(_bgmSoundKey, (_maxVol + _minVol) * 0.5f);
        _sfxSound = PlayerPrefs.GetFloat(_sfxSoundKey, (_maxVol + _minVol) * 0.5f);
    }

    private void Apply()
    {
        //_audioMixer.SetFloat(_masterMixerKey, _masterSound);
        _audioMixer.SetFloat(_bgmMixerKey, _bgmSound);
        _audioMixer.SetFloat(_sfxMixerKey, _sfxSound);
    }

    private void Save()
    {
        //PlayerPrefs.SetFloat(_masterSoundKey, _masterSound);
        PlayerPrefs.SetFloat(_bgmSoundKey, _bgmSound);
        PlayerPrefs.SetFloat(_sfxSoundKey, _sfxSound);
    }

    private void OnDestroy()
    {
        Save();
    }
}
