using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static Define;

public class ElevatorManager : MonoSingleTon<ElevatorManager>
{
    [SerializeField]
    private GameObject _elevatorUI = null;

    [SerializeField]
    private List<Elevator> _elevators = new List<Elevator>();
    private List<ElevatorOptionUI> _elevatorOptionUIs = new List<ElevatorOptionUI>();

    [SerializeField]
    private PlayableDirector _elevatorCutScene = null;
    [SerializeField]
    private Transform _elevatorOptionsParent = null;

    private int _curIndex = 0;
    public int CurIndex => _curIndex;
    private int _targetIndex = 0;


    private void Start()
    {
        _elevatorOptionUIs.AddRange(_elevatorOptionsParent.GetComponentsInChildren<ElevatorOptionUI>());
        for(int i = 0; i < _elevatorOptionUIs.Count; i++)
        {
            _elevators[i].MyIndex = i;
            _elevatorOptionUIs[i].Myindex = i;
            _elevatorOptionUIs[i].ButtonMapping();
        }
    }

    public void UIOpen()
    {
        for(int i = 0; i < _elevatorOptionUIs.Count; i++)
        {
            _elevatorOptionUIs[i].TextChange(_curIndex);
        }
        _elevatorUI.SetActive(true);
    }

    public void UIClose()
    {
        _elevatorUI.SetActive(false);
    }

    public void PlayCutScene()
    {
        _elevatorCutScene.Play();
    }

    public void CurElevatorSet(int index)
    {
        _curIndex = index;
        UIOpen();
    }

    public void TargetElevatorSet(int index)
    {
        _targetIndex = index;
    }

    public void PlayerPositionChange()
    {
        player.characterController.enabled = false;
        player.transform.position = _elevators[_targetIndex].EndPosition.position;
        player.characterController.enabled = true;
    }
}
