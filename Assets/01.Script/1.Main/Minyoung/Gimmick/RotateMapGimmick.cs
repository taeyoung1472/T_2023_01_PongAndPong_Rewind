using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RotateMapGimmick : ControlAbleObjcet
{
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float rotateAngle = 90f;
    [SerializeField] private Transform rotateTrm;

   // public bool locked = false;

    public Vector3 rotateVec;
    public Quaternion originRotation;

    public Quaternion targetRot;

    private void Start()
    {
        originRotation = rotateTrm.rotation;
    }
    public override void Control(ControlType controlType) //버튼이 눌리고있을때 나는 버튼을 눌렀을때가필요한것인가?
    {
        if (isLocked)
            return;
        switch (controlType)
        {
            case ControlType.Control:
                 rotateVec = rotateTrm.rotation.eulerAngles +  new Vector3(0, 0, rotateAngle);
                Debug.Log(rotateVec);
                RotateMap();
                break;
            case ControlType.ReberseControl:
                break;
        }
        curControlType = controlType;

    }
    public void RotateMap()
    {
        if (isLocked)
            return;
        isLocked = true;
        rotateTrm.DORotate(rotateVec, rotateSpeed).OnComplete(()=>
        {
            isLocked = false;
        });
        Debug.Log("너몇번실행되니");
    }
}
