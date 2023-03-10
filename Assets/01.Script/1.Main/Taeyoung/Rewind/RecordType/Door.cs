using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : TransformRecord, IFunctionalObject
{
    [SerializeField] private Vector3 moveValue;
    [SerializeField] private float speed = 1;
    Vector3 originPos;
    Vector3 openPos;
    bool isOpen = false;

    public override void Awake()
    {
        base.Awake();
        originPos = transform.localPosition;
        openPos = originPos + moveValue;
    }

    public void Function(bool isOn)
    {
        isOpen = isOn;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        transform.localPosition = Vector3.Lerp(transform.localPosition, isOpen ? openPos : originPos, Time.deltaTime * speed);
    }

#if UNITY_EDITOR

#endif
}
