using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform doorRight;
    [SerializeField] private Transform doorLeft;
    [SerializeField] private CinemachineVirtualCamera doorCam;
    
    public Transform playerPosition;

    public void Open(Action changePlayerPositionAction)
    {
        Sequence seq = DOTween.Sequence();

        doorCam.gameObject.SetActive(true);
        seq.Append(doorRight.DOLocalMoveX(1, 2f));
        seq.Join(doorLeft.DOLocalMoveX(-1, 2f));
        seq.AppendCallback(() => ElevatorController.Instance.ActiveBlackPanel());
        seq.AppendInterval(1.2f);
        seq.AppendCallback(() => changePlayerPositionAction?.Invoke());
        seq.AppendCallback(() => doorCam.gameObject.SetActive(false));
        seq.AppendInterval(2f);
        seq.AppendCallback(() =>
        {
            doorRight.transform.localPosition = Vector3.zero;
            doorLeft.transform.localPosition = Vector3.zero;
            doorCam.gameObject.SetActive(false);
            ElevatorController.Instance.DeActiveBlackPanel();
        });
    }
}
