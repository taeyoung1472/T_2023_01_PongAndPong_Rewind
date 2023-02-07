using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Player : MonoBehaviour
{
    private List<PlayerAction> _playerActions = new List<PlayerAction>();

    [SerializeField]
    private PlayerMovementSO _playerMovementSO = null;
    [SerializeField]
    private UnityEvent<bool> OnIsGrounded = null;

    private PlayerAnimation _playerAnimation = null;
    private GravityModule _gravityModule = null;
    private PlayerInput _playerInput = null;

    #region 프로퍼티
    public PlayerAnimation PlayerAnimation => _playerAnimation;
    public GravityModule GravityModule => _gravityModule;
    public PlayerInput PlayerInput => _playerInput;
    public PlayerMovementSO playerMovementSO => _playerMovementSO;
    #endregion

    private CharacterController _characterController = null;
    public CharacterController characterController => _characterController;
    private Vector3 _moveAmount = Vector3.zero;
    private Vector3 _extraMoveAmount = Vector3.zero;
    public bool IsGrounded => _characterController.isGrounded;
    private bool _lastGrounded = false;

    private void Awake()
    {
        List<PlayerAction> tempActions = new List<PlayerAction>(GetComponents<PlayerAction>());
        _playerActions = (from action in tempActions orderby action.ActionType ascending select action).ToList();
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _playerAnimation = transform.Find("AgentRenderer").GetComponent<PlayerAnimation>();
        _gravityModule = GetComponent<GravityModule>();
    }

    private void Update()
    {
        Move();
        GroundCheck();
    }

    public void VelocitySetMove(float? x = null, float? y = null)
    {
        if (x == null)
            x = _moveAmount.x;
        if (y == null)
            y = _moveAmount.y;
        _moveAmount = new Vector3(x.Value, y.Value, 0f);
    }

    public void VelocitySetExtra(float? x = null, float? y = null)
    {
        if (x == null)
            x = _extraMoveAmount.x;
        if (y == null)
            y = _extraMoveAmount.y;
        _extraMoveAmount = new Vector3(x.Value, y.Value, 0f);
    }

    public void VeloCityResetImm(bool x = false, bool y = false)
    {
        if (x)
        {
            _moveAmount.x = _extraMoveAmount.x = 0f;
        }
        if (y)
        {
            _moveAmount.y = _extraMoveAmount.y = 0f;
        }
    }

    /// <summary>
    /// 인자로 넘긴 액션들을 강제 종료
    /// </summary>
    /// <param name="types"></param>
    public void PlayerActionExit(params PlayerActionType[] types)
    {
        for (int i = 0; i < types.Length; i++)
            GetPlayerAction(types[i]).ActionExit();
    }

    /// <summary>
    /// 인자로 넘긴 액션들의 잠금을 value로 설정함
    /// </summary>
    /// <param name="value"></param>
    /// <param name="types"></param>
    public void PlayerActionLock(bool value, params PlayerActionType[] types)
    {
        for (int i = 0; i < types.Length; i++)
            GetPlayerAction(types[i]).Locked = value;
    }

    /// <summary>
    /// 인자로 넘긴 액션을 하고있는 중이라면 true 반환
    /// </summary>
    /// <param name="types"></param>
    /// <returns></returns>
    public bool PlayeActionCheck(params PlayerActionType[] types)
    {
        for (int i = 0; i < types.Length; i++)
            if (GetPlayerAction(types[i]).Excuting)
                return true;
        return false;
    }

    public PlayerAction GetPlayerAction(PlayerActionType type)
    {
        if (type == PlayerActionType.None)
        {
            Debug.LogError("type이 None인데?");
            return null;
        }
        return _playerActions[((int)type) - 1];
    }

    private void GroundCheck()
    {
        if (_lastGrounded == IsGrounded)
            return;
        else
            _lastGrounded = IsGrounded;

        OnIsGrounded?.Invoke(IsGrounded);
    }

    private void Move()
    {
        _characterController.Move((_moveAmount + _extraMoveAmount) * Time.deltaTime);
        _characterController.Move(_gravityModule.GetGravity() * Time.deltaTime);
    }
}
