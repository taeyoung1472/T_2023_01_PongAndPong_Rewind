using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip attackClip;
    public void PlayAttackAudio()
    {
        AudioRecord.Instance.PlayAudioRandPitch(attackClip);
    }
}
