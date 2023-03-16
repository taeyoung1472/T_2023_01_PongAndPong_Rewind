using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeField : MonoBehaviour
{
    public float bloomValue;
    public float weightVolume { get; private set; }
    [SerializeField] private float size;
    [SerializeField] private float outSize;

    private Transform eyeTransform;

    private void Start()
    {
        eyeTransform = GameObject.Find("eye").transform;
    }

    private void Update()
    {
        float dist = Mathf.Abs(transform.position.x - eyeTransform.position.x);
        if (dist < size)
        {
            weightVolume = 1;
        }
        else if (dist < size + outSize)
        {
            weightVolume = 1 - ((dist - size) / outSize);
        }
        else
            weightVolume = 0;
    }
}
