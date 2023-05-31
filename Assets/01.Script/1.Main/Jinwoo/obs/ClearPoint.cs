using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StageManager.Instance.GetCurArea().isAreaClear = true;
            AudioManager.PlayAudioRandPitch(SoundType.OnGameEnd);
        }
    }
}
