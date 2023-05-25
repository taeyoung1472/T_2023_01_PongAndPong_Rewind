using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VolumeManagerMinyoung : MonoBehaviour
{
    [SerializeField]
    private Slider _bgmSlider = null;
    [SerializeField]
    private Slider _sfxSlider = null;

    private AudioApplySound _applyAudioSound = null;
    public void Start()
    {
        if (_applyAudioSound == null)
        {
            _applyAudioSound = GetComponent<AudioApplySound>();
        }
        Init();
    }

    private void Init()
    {
        SetMaxMinValue(_bgmSlider);
        SetMaxMinValue(_sfxSlider);

        _bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        _bgmSlider.value = _applyAudioSound.BGMSound;
        _sfxSlider.value = _applyAudioSound.SfxSound;
    }

    private void SetMaxMinValue(Slider target)
    {
        target.maxValue = _applyAudioSound.MaxVol;
        target.minValue = _applyAudioSound.MinVol;
    }

    public void SetBGMVolume(float value)
    {
        _applyAudioSound.BGMSound = value;
    }
    public void SetSFXVolume(float value)
    {
        _applyAudioSound.SfxSound = value;
    }
}
