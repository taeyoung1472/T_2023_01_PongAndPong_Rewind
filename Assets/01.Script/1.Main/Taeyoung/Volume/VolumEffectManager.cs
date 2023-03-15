using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumEffectManager : MonoBehaviour
{
    [SerializeField] private Volume targetVolume;
    public List<VolumeField> volumeFieldList = new();

    private Bloom bloom;

    public void Awake()
    {
        if (!targetVolume.profile.TryGet<Bloom>(out bloom))
            Debug.LogWarning("Bloom has not override");
    }

    public void Update()
    {
        CalculVolume();
    }

    private void CalculVolume()
    {
        float bloomIntensity = 0;

        foreach (var field in volumeFieldList)
        {
            bloomIntensity += field.bloomValue * field.weightVolume;
        }

        bloom.intensity.value = bloomIntensity;
    }
}
