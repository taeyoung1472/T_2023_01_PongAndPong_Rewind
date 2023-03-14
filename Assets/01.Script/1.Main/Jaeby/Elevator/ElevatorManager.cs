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
    private List<ElevatorInteract> _elevators = new List<ElevatorInteract>();
    [SerializeField]
    private ElevatorOptionUI _elevatorOptionUIPrefab = null;
    [SerializeField]
    private Transform _optionUIParent = null;

    private List<ElevatorOptionUI> _elevatorOptionUIs = new List<ElevatorOptionUI>();

    [SerializeField]
    private PlayableDirector _elevatorCutScene = null;

    private ElevatorInteract _curElevator = null;
    private ElevatorInteract _targetElevator = null;

    public ElevatorInteract CurElevator { get => _curElevator; set => _curElevator = value; }
    public ElevatorInteract TargetElevator { get => _targetElevator; set => _targetElevator = value; }

    private void Start()
    {
        for(int i = 0; i < _elevators.Count; i++)
        {
            ElevatorOptionUI option = Instantiate(_elevatorOptionUIPrefab, _optionUIParent);
            option.Elevator = _elevators[i];
            option.OptionUIInit(_elevators[i].AreaName);
            _elevatorOptionUIs.Add(option);
        }
        _elevatorUI.SetActive(false);
    }

    public void ElevatorInit(ElevatorInteract curElevator)
    {
        _curElevator = curElevator;
        for(int i = 0; i < _elevatorOptionUIs.Count; i++)
        {
            _elevatorOptionUIs[i].TextChange(_curElevator);
        }
        _elevatorUI.SetActive(true);
    }

    public void PlayCutScene()
    {
        _elevatorCutScene.Play();
        _elevatorUI.SetActive(false);
    }

    public void PlayerPositionChangeToTarget()
    {
        player.characterController.enabled = false;
        player.transform.position =TargetElevator.PlayerPosition.position;
        player.characterController.enabled = true;
    }

    public void ElevatorInteractEnd()
    {
        _curElevator.InteractEnd(true);
        _curElevator = _targetElevator = null;
    }
}
