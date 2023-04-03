using DigitalRuby.ThunderAndLightning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidController : MonoBehaviour
{
    [SerializeField] private LightningBoltPrefabScript[] voids;

    [SerializeField, Range(0.0f, 1.0f)] private float intencity;
    
    private void Update()
    {
        Set(intencity);
    }

    public void Set(float percent)
    {
        foreach (var vd in voids)
        {
            vd.GlowIntensity = Mathf.Lerp(0.0f, 10.0f, percent);
        }
    }
}
