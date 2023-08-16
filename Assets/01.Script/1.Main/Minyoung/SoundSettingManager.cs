using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingManager : MonoBehaviour
{
    public Slider audioSlider;

    public void OffSound()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }
    public void AudioControl()
    {
        float sound = audioSlider.value;

        if (sound == -40f) AudioManager.Mixer.SetFloat("Master", -80);
        else
        {
            AudioManager.Mixer.SetFloat("Master", sound);
        }


    }
}
