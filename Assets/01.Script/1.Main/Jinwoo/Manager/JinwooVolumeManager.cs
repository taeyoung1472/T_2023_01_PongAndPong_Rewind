using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class JinwooVolumeManager : MonoSingleTon<JinwooVolumeManager>
{
    public Volume volume;

    private CinematicBars cinematicBars;
    private LimitlessGlitch1 glitch1;
    private LimitlessGlitch2 glitch2;
    private LimitlessGlitch3 glitch3;

    private Noise noise;
    private TVEffect tvEffect;

    private float barAmount = 0.15f;
    private void Start()
    {
        volume.profile.TryGet(out cinematicBars);
        volume.profile.TryGet(out glitch1);
        volume.profile.TryGet(out glitch2);
        volume.profile.TryGet(out glitch3);

        volume.profile.TryGet(out noise);
        volume.profile.TryGet(out tvEffect);

        cinematicBars.enable.value = false;
        glitch1.enable.value = false;
        glitch2.enable.value = false;
        glitch3.enable.value = false;

        noise.enable.value = false;
        tvEffect.enable.value = false;

        barAmount = 0.15f;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartMadGlitch(true);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartMadGlitch(false);
        }
    }
    public void EnableGlitch()
    {
        glitch1.enable.value = true;
        glitch2.enable.value = true;
        glitch3.enable.value = true;
    }
    public void DisableGlitch()
    {
        glitch1.enable.value = false;
        glitch2.enable.value = false;
        glitch3.enable.value = false;
    }

    public void DirectDisableCinematicBars()
    {
        cinematicBars.amount.value = 0.01f;
        cinematicBars.enable.value = false;
    }
    public void StartFadeInCinematicBars()
    {
        StartCoroutine(FadeInCinematicBars());
    }
    public void StartFadeOutCinematicBars(bool isEnable = false)
    {
        StartCoroutine(FadeOutCinematicBars(isEnable));

    }
    public IEnumerator FadeInCinematicBars()
    {
        cinematicBars.enable.value = true;
        cinematicBars.amount.value = 0.01f;
        while (cinematicBars.amount.value < cinematicBars.amount.max)
        {
            cinematicBars.amount.value += 0.01f;
            yield return new WaitForSeconds(0.02f);
        }
    }
    public IEnumerator FadeOutCinematicBars(bool isEnable = false)
    {
        cinematicBars.enable.value = true;
        cinematicBars.amount.value = 0.51f;
        float minAmount = 0.01f;
        if (isEnable)
        {
            minAmount = 0.15f;
        }
        while (cinematicBars.amount.value >= minAmount)
        {
            cinematicBars.amount.value -= 0.01f;
            yield return new WaitForSeconds(0.02f);
        }
    }
    public void StartCinematicBars()
    {
        StartCoroutine(EnableCinematicBars());
    }
    public void EndCinmaticBars()
    {
        StartCoroutine(DisableCinematicBars());

    }
    public IEnumerator EnableCinematicBars()
    {
        cinematicBars.enable.value = true;
        cinematicBars.amount.value = 0.01f;
        while (cinematicBars.amount.value <= barAmount)
        {
            cinematicBars.amount.value += 0.01f;
            yield return new WaitForSeconds(0.05f);
        }
    }
    public IEnumerator DisableCinematicBars()
    {
        while (cinematicBars.amount.value >= 0.01f)
        {
            cinematicBars.amount.value -= 0.01f;
            yield return new WaitForSeconds(0.05f);
        }
        cinematicBars.enable.value = false;
    }

    public void EnableFastTimeEffect()
    {
        if(noise != null && tvEffect != null)
        {
            noise.enable.value = true;
            tvEffect.enable.value = true;
        }
        
        
    }
    public void DisableFastTimeEffect()
    {
        if (noise != null && tvEffect != null)
        {
            noise.enable.value = false;
            tvEffect.enable.value = false;
        }

    }
    public void EnableCCTVVolume(bool isOn)
    {
        if (cinematicBars.enable.value)
        {
            cinematicBars.enable.value = false;
        }
        if (noise != null)
        {
            noise.enable.value = isOn;
        }
    }
    public void StartMadGlitch(bool isOn)
    {
        StopAllCoroutines();
        StartCoroutine(Glitch2(isOn));
    }
    public IEnumerator Glitch2(bool isOn)
    {
        if (isOn)
        {
            glitch2.enable.value = true;
            glitch2.intensity.value = 1f;
            while (glitch2.intensity.value >= 0f)
            {
                glitch2.intensity.value -= 0.05f;
                yield return new WaitForSeconds(0.02f);
            }
        }
        else
        {
            glitch2.enable.value = true;
            glitch2.intensity.value = 0f;
            while (glitch2.intensity.value <= 1f)
            {
                glitch2.intensity.value += 0.05f;
                yield return new WaitForSeconds(0.02f);
            }
            glitch2.enable.value = false;
        }
       
    }
}
