using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenericRewind))]
public class MovingObjectGimmick : ControlAbleObjcet
{
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 rotateDir;
    [SerializeField] private StageArea myArea;
    private bool isMoving;

    Vector3 originPos;
    Quaternion originRot;

    public void Awake()
    {
        originPos = transform.position;
        originRot = transform.rotation;
        myArea.OnExitArea += () =>
        {
            transform.SetPositionAndRotation(originPos, originRot);
            this.enabled = false;
        };
        myArea.OnEntryArea += () =>
        {
            isMoving = true;
            this.enabled = true;
        };
    }

    public override void Control(ControlType controlType, bool isLever, Player player, DirectionType dirType)
    {

    }

    public override void ResetObject()
    {

    }

    public void Update()
    {
        if (isMoving)
        {
            transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;
            transform.Rotate(rotateDir * Time.deltaTime);
        }
    }
}
