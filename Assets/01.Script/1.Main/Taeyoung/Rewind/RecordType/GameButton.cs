using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressButton : TransformRecord
{
    private bool isActive = false;
    [SerializeField] private UnityEvent funtion;

    public override void InitOnPlay()
    {
        base.InitOnPlay();
        isActive = true;
    }
    public override void InitOnRewind()
    {
        base.InitOnRewind();
        isActive = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {

        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {

        }
    }
}
