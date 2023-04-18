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
    public override void Control(ControlType controlType) //��ư�� ������������ ���� ��ư�� �����������ʿ��Ѱ��ΰ�?
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
        Debug.Log("�ʸ������Ǵ�");
    }
}
