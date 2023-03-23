using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnviromentSound : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1.0f)] private float soundLimit = 1f;
    AudioSource audioSource = null;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume * soundLimit;
    }
}
