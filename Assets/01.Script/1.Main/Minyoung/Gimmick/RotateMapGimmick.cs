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
    public override void Control(ControlType controlType, bool isLever, Player player, DirectionType dirType) //��ư�� ������������ ���� ��ư�� �����������ʿ��Ѱ��ΰ�?
    {
        if (isLocked)
            return;

        switch (controlType)
        {
            case ControlType.Control:
                rotateVec = rotateTrm.rotation.eulerAngles + new Vector3(0, 0, rotateAngle);
                Debug.Log(rotateVec);
                if (isLever)
                {
                    RotateMap(rotateVec, player);
                }
                else
                {
                    RotateMap(Vector3.zero, player);
                }

                break;
            case ControlType.ReberseControl:
                break;
        }
        curControlType = controlType;

    }
    public void RotateMap(Vector3 rotateVec, Player player)
    {
        if (isLocked)
            return;
        isLocked = true;
        player.GravityModule.GravityScale = 0.6f;
        rotateTrm.DORotate(rotateVec, rotateSpeed).OnComplete(() =>
        {
            //�����¤���
            if(player != null)
                player.GravityModule.GravityScale = 0.8f;
            isLocked = false;
        });
        Debug.Log("������Ʈ��" + rotateVec);
    }
}
