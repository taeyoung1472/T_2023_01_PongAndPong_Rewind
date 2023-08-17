using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingManager : MonoBehaviour
{
    public Slider audioSlider;
    public bool isMute = false;
    public float sound = 0f;

    public GameObject off;
    public GameObject on;
    public void Start()
    {
        SaveDataManager.Instance.LoadSoundJSON();

        isMute = SaveDataManager.Instance.SettingValue.isMute;
        sound = SaveDataManager.Instance.SettingValue.volume;

        InitUI();
    }

    public void InitUI()
    {
        if (isMute)
        {
            off.SetActive(true);
            on.SetActive(false);
        }
        else
        {
            off.SetActive(false);
            on.SetActive(true);
        }

        audioSlider.value = sound;


        SoundChange();
    }

    public void OffSound()
    {
        isMute = !isMute;
    }
    public void AudioControl()
    {
        sound = audioSlider.value;
    }
    public void SoundChange()
    {
        if (isMute)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }

        if (sound == -40f) AudioManager.Mixer.SetFloat("Master", -80);
        else
        {
            AudioManager.Mixer.SetFloat("Master", sound);
        }
    }


    public void SaveSetting()
    {
        //뮤트
        SoundChange();

        //음량조절
        SoundChange();

        SaveDataManager.Instance.SettingValue.isMute = isMute;
        SaveDataManager.Instance.SettingValue.volume = sound;

        SaveDataManager.Instance.SaveSoundJSON(); 
    }
}
