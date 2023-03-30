using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.IO;

public class Player : MonoBehaviour
{
    private void OnEnable()
    {
        CamManager.Instance.AddTargetGroup(transform);
    }

    private void OnDisable()
    {
        CamManager.Instance.RemoveTargetGroup(transform);
    }

    private List<PlayerAction> _playerActions = new List<PlayerAction>();

    #region SO
    [SerializeField]
    private PlayerHealthSO _playerHealthSO = null;
    [SerializeField]
    private PlayerMovementSO _playerMovementSO = null;
    [SerializeField]
    private PlayerAttackSO _playerAttackSO = null;
    [SerializeField]
    private UnityEvent<bool> OnIsGrounded = null;
    #endregion

    #region 캐싱 데이터
    private PlayerAnimation _playerAnimation = null;
    private PlayerRenderer _playerRenderer = null;
    private GravityModule _gravityModule = null;
    private PlayerInput _playerInput = null;
    private CharacterController _characterController = null;
    private PlayerTrail _playerTrail = null;
    private PlayerAudio _playerAudio = null;
    private PlayerBuff _playerBuff = null;
    private PlayerHP _playerHP = null;
    #endregion

    #region 프로퍼티
    public PlayerRenderer PlayerRenderer => _playerRenderer;
    public PlayerAnimation PlayerAnimation => _playerAnimation;
    public GravityModule GravityModule => _gravityModule;
    public PlayerInput PlayerInput => _playerInput;
    public PlayerMovementSO playerMovementSO => _playerMovementSO;
    public PlayerAttackSO playerAttackSO => _playerAttackSO;
    public PlayerHealthSO playerHealthSO => _playerHealthSO;
    public CharacterController characterController => _characterController;
    public PlayerAudio playerAudio => _playerAudio;
    public PlayerBuff playerBuff => _playerBuff;
    public PlayerHP playerHP => _playerHP;
    #endregion

    #region Json 저장 데이터
    private PlayerJsonData _playerJsonData = null;
    public PlayerJsonData playerJsonData => _playerJsonData;
    private PlayerInventory _playerInventory = null;
    public PlayerInventory playerInventory => _playerInventory;
    #endregion

    #region 이동 관련
    private Vector3 _moveAmount = Vector3.zero;
    public Vector3 MoveAmount => _moveAmount;
    private Vector3 _extraMoveAmount = Vector3.zero;
    public Vector3 ExtraMoveAmount => _extraMoveAmount;
    private Vector3 _characterMoveAmount = Vector3.zero;
    public Vector3 CharacterMoveAmount => _characterMoveAmount;
    private CollisionFlags _collisionFlag = CollisionFlags.None;
    #endregion

    #region 바닥 체크용 박스캐스트 관련
    [SerializeField]
    private float _groundCheckRayLength = 0.225f;
    [SerializeField]
    private LayerMask _groundMask = 0;
    private Collider _col = null;
    private bool _isGrounded = false;
    public bool IsGrounded => _isGrounded;
    #endregion

    private void Awake()
    {
        LoadJson();
        //액션 초기화
        List<PlayerAction> tempActions = new List<PlayerAction>(GetComponents<PlayerAction>());
        _playerActions = (from action in tempActions orderby action.ActionType ascending select action).ToList();
        //캐싱
        _playerBuff = GetComponent<PlayerBuff>();
        _playerHP = GetComponent<PlayerHP>();
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _playerAnimation = transform.Find("AgentRenderer").GetComponent<PlayerAnimation>();
        _playerRenderer = _playerAnimation.GetComponent<PlayerRenderer>();
        _gravityModule = GetComponent<GravityModule>();
        _col = GetComponent<Collider>();
        _playerTrail = GetComponent<PlayerTrail>();
        _playerAudio = transform.Find("AgentSound").GetComponent<PlayerAudio>();
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
    }

    private void FixedUpdate()
    {
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

        if (_playerActions.Count == (int)PlayerActionType.Size - 1) // 다 있음
        {
            return _playerActions[((int)type) - 1];
        }
        else
        {
            for (int i = 0; i < _playerActions.Count; i++)
            {
                if (_playerActions[i].ActionType == type)
                    return _playerActions[i];
            }
        }

        return null;
    }

    public T GetPlayerAction<T>() where T : PlayerAction
    {
        for (int i = 0; i < _playerActions.Count; i++)
        {
            if (_playerActions[i] is T)
                return _playerActions[i] as T;
        }
        return null;
    }

    private void GroundCheck()
    {
        bool lastGrounded = _isGrounded;
        Vector3 boxCenter = _col.bounds.center;
        Vector3 halfExtents = _col.bounds.extents;
        halfExtents.y = _groundCheckRayLength;
        float maxDistance = _col.bounds.extents.y;
        _isGrounded = Physics.BoxCast(boxCenter, halfExtents, -transform.up, transform.rotation, maxDistance, _groundMask);
        if (lastGrounded == _isGrounded)
            return;
        OnIsGrounded?.Invoke(_isGrounded);
    }

    private void Move()
    {
        _characterMoveAmount = ((_moveAmount + _extraMoveAmount) +
            ((_isGrounded == false && _gravityModule.UseGravity) ? _gravityModule.GetGravity() : Vector3.zero))
            ;
        if (_characterController.velocity.y < 0f)
        {
            _characterMoveAmount += Vector3.up * GravityModule.GetGravity().y * (_playerMovementSO.fallMultiplier - 1) * Time.deltaTime;
        }
        _collisionFlag = _characterController.Move(_characterMoveAmount * Time.deltaTime);
    }

    public void PlayerInteractActionExit()
    {
        PlayerActionExit(PlayerActionType.Interact);
    }

    public void ForceStop()
    {
        PlayerActionExit(PlayerActionType.Move, PlayerActionType.Dash, PlayerActionType.Jump);
        VeloCityResetImm(true, true);
        _playerInput.InputVectorReset();
        _playerAnimation.FallOrIdleAnimation(IsGrounded);
    }

    public void AfterImageEnable(bool value)
    {
        if (_playerTrail == null)
            return;

        if (value)
            _playerTrail.StartTrail();
        else
            _playerTrail.EndTrail();
    }
}
