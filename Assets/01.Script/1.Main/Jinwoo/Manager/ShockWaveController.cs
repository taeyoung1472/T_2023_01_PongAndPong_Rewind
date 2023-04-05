using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveController : MonoBehaviour
{
    private Animator anime;

    private void Awake()
    {
        anime = GetComponent<Animator>();
    }

    public void StartShockWave()
    {
        anime.Play("startShock");
    }

}
