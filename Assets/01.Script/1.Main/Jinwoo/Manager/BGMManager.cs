using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoSingleTon<BGMManager> 
{
    public AudioSource audioSource;
    public AudioClip[] bgmClip;
    

    public void BgSoundPlay(AudioClip clip, float volume)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void BgSoundPlay(int num)
    {
        audioSource.clip = bgmClip[num];
        //audioSource.volume = 0.4f;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void StopBGM()
    {
        if(audioSource.isPlaying)
        {
            audioSource.Stop();

        }
    }

}
