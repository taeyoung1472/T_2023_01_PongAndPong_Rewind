using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCurveInteract : Interact
{
    private ShootLaser shootLaser;

    public float rotateValue = 45f;
    private void Awake()
    {
        shootLaser = FindObjectOfType<ShootLaser>();   
    }

    protected override void ChildInteractEnd()
    {
    }

    protected override void ChildInteractStart()
    {
        transform.Rotate(Vector3.up * rotateValue * Time.deltaTime);
        shootLaser.SetLine();
        InteractEnd(true);
    }
}
