using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField]
    private Transform _endPosition = null;
    private int _myIndex = 0;

    public Transform EndPosition => _endPosition;
    public int MyIndex { get => _myIndex; set => _myIndex = value; }


}
