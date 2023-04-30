using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class JinwooVolumeManager : MonoSingleTon<JinwooVolumeManager>
{
    public Volume volume;

    private CinematicBars cinematicBars;

    [SerializeField] private float barAmount;
    private void Start()
    {
        volume.profile.TryGet(out cinematicBars);
        cinematicBars.enable.value = false;
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
