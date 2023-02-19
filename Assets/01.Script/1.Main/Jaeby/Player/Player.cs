using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.IO;

public class Player : MonoBehaviour
{
    private List<PlayerAction> _playerActions = new List<PlayerAction>();

    [SerializeField]
    private PlayerMovementSO _playerMovementSO = null;
    [SerializeField]
    private PlayerAttackSO _playerAttackSO = null;
    [SerializeField]
    private UnityEvent<bool> OnIsGrounded = null;

    private PlayerAnimation _playerAnimation = null;
    private PlayerRenderer _playerRenderer = null;
    private GravityModule _gravityModule = null;
    private PlayerInput _playerInput = null;

    #region 프로퍼티
    public PlayerRenderer PlayerRenderer => _playerRenderer;
    public PlayerAnimation PlayerAnimation => _playerAnimation;
    public GravityModule GravityModule => _gravityModule;
    public PlayerInput PlayerInput => _playerInput;
    public PlayerMovementSO playerMovementSO => _playerMovementSO;
    public PlayerAttackSO playerAttackSO => _playerAttackSO;
    #endregion

    private PlayerJsonData _playerJsonData = null;
    public PlayerJsonData playerJsonData => _playerJsonData;
    private PlayerInventory _playerInventory = null;
    public PlayerInventory playerInventory => _playerInventory;

    private CharacterController _characterController = null;
    public CharacterController characterController => _characterController;
    private Vector3 _moveAmount = Vector3.zero;
    private Vector3 _extraMoveAmount = Vector3.zero;
    public bool IsGrounded => _collisionFlag == CollisionFlags.Below;
    private CollisionFlags _collisionFlag = CollisionFlags.None;
    private bool _lastGrounded = false;

    private void Awake()
    {
        LoadJson();
        List<PlayerAction> tempActions = new List<PlayerAction>(GetComponents<PlayerAction>());
        _playerActions = (from action in tempActions orderby action.ActionType ascending select action).ToList();
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _playerAnimation = transform.Find("AgentRenderer").GetComponent<PlayerAnimation>();
        _playerRenderer = _playerAnimation.GetComponent<PlayerRenderer>();
        _gravityModule = GetComponent<GravityModule>();
    }

    private void LoadJson()
    {
        string path = Application.dataPath + "/Save/JsonData.json";
        string json = File.ReadAllText(path);
        _playerJsonData = JsonUtility.FromJson<PlayerJsonData>(json);
        if (_playerJsonData == null)
            _playerJsonData = new PlayerJsonData();
        _playerInventory = GetComponent<PlayerInventory>();
        _playerInventory.LoadInventory();
    }

    [ContextMenu("데이터 세이브")]
    public void SaveJsonData()
    {
        string path = Application.dataPath + "/Save/JsonData.json";
        string json = JsonUtility.ToJson(_playerJsonData);
        File.WriteAllText(path, json);
        _playerInventory.SaveInventory();
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
    /// 인자로 넘긴 액션이 잠겨있다면 true 반환
    /// </summary>
    /// <param name="types"></param>
    /// <returns></returns>
    public bool PlayerActionLockCheck(params PlayerActionType[] types)
    {
        for (int i = 0; i < types.Length; i++)
            if (GetPlayerAction(types[i]).Locked)
                return true;
        return false;
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
        _lastGrounded = IsGrounded;
        OnIsGrounded?.Invoke(IsGrounded);
    }

    private void Move()
    {
        _collisionFlag = _characterController.Move((_moveAmount + _extraMoveAmount + 
            ((IsGrounded == false && _gravityModule.UseGravity) ? _gravityModule.GetGravity() : Vector3.zero))
            * Time.deltaTime);
    }

    public void PlayerInteractActionExit()
    {
        PlayerActionExit(PlayerActionType.Interact);
    }
}
