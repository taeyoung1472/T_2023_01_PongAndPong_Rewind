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

    public override void InteractEnter()
    {
        UIGetter.Instance.GetInteractUI(_interactUIPos.position, _interactSprite, KeyManager.keys[InputType.Interact]);

    }

    public override void InteractExit()
    {
        UIGetter.Instance.PushUIs();
    }
}
