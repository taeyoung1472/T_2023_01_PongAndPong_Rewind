using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTimeImgFade : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animator.Play("FastTimeFade");
    }


}
