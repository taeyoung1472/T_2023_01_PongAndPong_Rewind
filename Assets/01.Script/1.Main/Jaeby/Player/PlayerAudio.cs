using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public void MoveAudio()
    {
        AudioManager.PlayAudio(SoundType.Footstep);
    }
    public void AttackAudio()
    {
        AudioManager.PlayAudioRandPitch(SoundType.Attack);
    }
    public void OnGroundedAudio()
    {
        AudioManager.PlayAudioRandPitch(SoundType.OnGrounded);
    }

    public void JumpAudio()
    {
        AudioManager.PlayAudioRandPitch(SoundType.Jump);
    }
    public void DashAirAudio()
    {
        AudioManager.PlayAudioRandPitch(SoundType.DashAir);
    }
    public void DashGroundAudio()
    {
        AudioManager.PlayAudioRandPitch(SoundType.DashGround);
    }
}
