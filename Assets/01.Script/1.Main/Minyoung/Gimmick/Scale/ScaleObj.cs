using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObj : ControlAbleObjcet
{
    Rigidbody rb;
    Vector3 originPos;
    Quaternion originRot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originPos = transform.position;
        originRot = transform.rotation;
        RewindManager.Instance.InitRewind += () =>
        {
            rb.useGravity = false;
            transform.SetPositionAndRotation(originPos, originRot);
            this.enabled = false;
        };
    }
    public override void Control(ControlType controlType, bool isLever, Player player, DirectionType dirType)
    {
        switch (controlType)
        {
            case ControlType.Control:
                rb.useGravity = true;
                break;
            case ControlType.None:
                rb.useGravity = false;
                break;
            case ControlType.ReberseControl:
                break;
        }

    }
}
