using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObj : ControlAbleObjcet
{
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();   
    }
    public override void Control(ControlType controlType, bool isLever, Player player, DirectionType dirType)
    {
        switch (controlType)
        {
            case ControlType.Control:
            Debug.Log("이거작동중임");
                rb.useGravity = true;
                break;
            case ControlType.None:
                break;
            case ControlType.ReberseControl:
                break;
        }

    }
}
