using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField]
    private Slider _masterSlider = null;
    [SerializeField]
    private Slider _bgmSlider = null;
    [SerializeField]
    private Slider _sfxSlider = null;

    private AudioApplySound _applyAudioSound = null;


    public GameObject volumeUI;

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
        //SetMaxMinValue(_masterSlider);
        SetMaxMinValue(_bgmSlider);
        SetMaxMinValue(_sfxSlider);

        //_masterSlider.onValueChanged.AddListener(SetMasterVolume);
        _bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        //_masterSlider.value = _applyAudioSound.MasterSound;
        _bgmSlider.value = _applyAudioSound.BGMSound;
        _sfxSlider.value = _applyAudioSound.SfxSound;
    }

    private void SetMaxMinValue(Slider target)
    {
        target.maxValue = _applyAudioSound.MaxVol;
        target.minValue = _applyAudioSound.MinVol;
    }

    public void SetMasterVolume(float value)
    {
        //_applyAudioSound.MasterSound = value;
    }
    public void SetBGMVolume(float value)
    {
        _applyAudioSound.BGMSound = value;
    }
    public void SetSFXVolume(float value)
    {
        _applyAudioSound.SfxSound = value;
    }

    public void ClickVolume()
    {
        volumeUI.SetActive(true);
    }
    public void ExitVolume()
    {
        volumeUI.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ExitScene()
    {
        SceneManager.LoadScene("Start");
    }
}
