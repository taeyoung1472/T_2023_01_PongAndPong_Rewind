using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleCnt : MonoBehaviour
{
    [SerializeField] private Material mat;

    [SerializeField]
    [Range(0f, 1f)]
    private float range = 0f;
    [SerializeField]
    [Range(0f, 2f)]
    private float intensity = 0f;
    void Start()
    {
        
    }

    void Update()
    {
        mat.SetFloat("_Range", range);
        mat.SetFloat("_Color_Intensity", intensity);
    }
}
