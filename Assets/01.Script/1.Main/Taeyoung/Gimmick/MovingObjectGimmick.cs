using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenericRewind))]
public class MovingObjectGimmick : ControlAbleObjcet
{
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 rotateDir;
    private bool isMoving;

    public void Awake()
    {
        RewindManager.Instance.InitRewind += () => this.enabled = false;
        RewindManager.Instance.InitPlay += () =>
        {
            isMoving = true;
            this.enabled = true;
        };
    }

    public override void Control(ControlType controlType, bool isLever, Player player, DirectionType dirType)
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
