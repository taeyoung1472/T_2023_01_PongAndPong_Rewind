using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class JinwooVolumeManager : MonoSingleTon<JinwooVolumeManager>
{
    public Volume volume;

    private CinematicBars cinematicBars;
    private LimitlessGlitch1 glitch1;
    private LimitlessGlitch2 glitch2;
    private LimitlessGlitch3 glitch3;

    [SerializeField] private float barAmount;
    private void Start()
    {
        volume.profile.TryGet(out cinematicBars);
        volume.profile.TryGet(out glitch1);
        volume.profile.TryGet(out glitch2);
        volume.profile.TryGet(out glitch3);
        cinematicBars.enable.value = false;
        glitch1.enable.value = false;
        glitch2.enable.value = false;
        glitch3.enable.value = false;
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


}
