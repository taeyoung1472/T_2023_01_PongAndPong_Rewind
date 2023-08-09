using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorController : MonoSingleTon<ElevatorController>
{
    [SerializeField] private Image blackPanel;

    [SerializeField] private Transform player;
    [SerializeField] private List<Elevator> elevatorList = new();
    private int myFloor;

    public void UseElevator(int targetFloor)
    {
        elevatorList[myFloor].Open(() => player.position = elevatorList[targetFloor].playerPosition.position);
        myFloor = targetFloor;
    }

    public void ActiveBlackPanel()
    {
        Debug.Log("Black On");
        blackPanel.DOColor(new Color(0, 0, 0, 1), 1f);
    }

    public void DeActiveBlackPanel()
    {
        Debug.Log("Black Off");
        blackPanel.DOColor(new Color(0, 0, 0, 0), 1f);
    }
}
