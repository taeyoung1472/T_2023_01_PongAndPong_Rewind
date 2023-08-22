using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSucking : MonoBehaviour
{
    [SerializeField] private List<Material> _mats = new List<Material>();

    [SerializeField]
    [Range(0f, 1f)]
    private float progress = 1f;
    void Start()
    {
        
    }

    void Update()
    {
        foreach (Material mat in _mats)
        {
            mat.SetFloat("_Dissolve", progress);
        }
    }
}
