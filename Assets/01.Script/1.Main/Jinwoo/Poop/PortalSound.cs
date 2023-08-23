using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSound : MonoBehaviour
{
    [SerializeField] private bool isOn = false;
    private void Start()
    {
        isOn = false;
    }
    private void OnDisable()
    {
        isOn = false;
    }
    public void PlaySound()
    {
        if (isOn)
        {
            return;
        }
        isOn = true;
        StopAllCoroutines();
        StartCoroutine(PlayPortalSound());
    }
    public IEnumerator PlayPortalSound()
    {
        isOn = true;
        yield return new WaitForSeconds(0.65f);
        isOn = true;
        AudioManager.PlayAudioRandPitch(SoundType.OnPortalSound);

    }
}
