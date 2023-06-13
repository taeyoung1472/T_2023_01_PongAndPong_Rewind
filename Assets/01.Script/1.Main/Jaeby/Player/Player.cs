using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.IO;
using System;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    private List<PlayerAction> _playerActions = new List<PlayerAction>();
    private List<IPlayerEnableResetable> _enableResetables = new List<IPlayerEnableResetable>();
    private List<IPlayerDisableResetable> _disableResetables = new List<IPlayerDisableResetable>();

    #region SO
    [SerializeField]
    private StageDatabase _worldsDatabase = null;

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
    private TrailableObject _playerTrail = null;
    private Rigidbody _rigid = null;
    private PlayerAudio _playerAudio = null;
    private PlayerBuff _playerBuff = null;
    private PlayerHP _playerHP = null;
    private CapsuleCollider _col = null;
    private AnimationIK _animationIK = null;
    #endregion

    #region 프로퍼티
    public PlayerRenderer PlayerRenderer => _playerRenderer;
    public PlayerAnimation PlayerAnimation => _playerAnimation;
    public GravityModule GravityModule => _gravityModule;
    public PlayerInput PlayerInput => _playerInput;
    public PlayerMovementSO playerMovementSO => _playerMovementSO;
    public PlayerAttackSO playerAttackSO => _playerAttackSO;
    public PlayerHealthSO playerHealthSO => _playerHealthSO;
    public Rigidbody Rigid => _rigid;
    public TrailableObject playerTrail => _playerTrail;
    public PlayerAudio playerAudio => _playerAudio;
    public PlayerBuff playerBuff => _playerBuff;
    public PlayerHP playerHP => _playerHP;
    public CapsuleCollider Col => _col;
    public AnimationIK animationIK => _animationIK;
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
    #endregion

    #region 바닥 체크용 박스캐스트 관련
    [SerializeField]
    private float _groundCheckRayLength = 0.225f;
    [SerializeField]
    private LayerMask _groundMask = 0;
    private bool _isGrounded = false;
    public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }
    #endregion

    #region 경사면 관련
    RaycastHit _slopeHit;
    #endregion

    private void Awake()
    {
        LoadJson();
        //액션 초기화
        List<PlayerAction> tempActions = new List<PlayerAction>(GetComponents<PlayerAction>());
        _playerActions = (from action in tempActions orderby action.ActionType ascending select action).ToList();
        _enableResetables = new List<IPlayerEnableResetable>(GetComponents<IPlayerEnableResetable>());
        _disableResetables = new List<IPlayerDisableResetable>(GetComponents<IPlayerDisableResetable>());
        //캐싱
        _playerBuff = GetComponent<PlayerBuff>();
        _playerHP = GetComponent<PlayerHP>();
        _rigid = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _gravityModule = GetComponent<GravityModule>();
        _playerTrail = GetComponent<TrailableObject>();
        _col = GetComponent<CapsuleCollider>();

        _playerAudio = transform.Find("AgentSound").GetComponent<PlayerAudio>();
        _playerRenderer = transform.Find("AgentRenderer").GetComponent<PlayerRenderer>(); ;
        _playerAnimation = _playerRenderer.GetComponent<PlayerAnimation>();
        _animationIK = _playerRenderer.GetComponent<AnimationIK>();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Move();
    }

    #region 플레이어 이동
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

    private void Move()
    {
        _characterMoveAmount = ((_moveAmount + _extraMoveAmount) +
            ((_isGrounded == false && _gravityModule.UseGravity) ? _gravityModule.GetGravity() : Vector3.zero))
            ;
        if (_isGrounded == false)
        {
            // 중력 방향에 따라 다르게
            _characterMoveAmount += transform.up * GravityModule.GetGravity().y * Time.deltaTime
                * ((_rigid.velocity.y <= -0.5f) ? _playerMovementSO.fallMultiplier : _playerMovementSO.upMultiplier);
        }
        if (OnSlope())
        {
            _characterMoveAmount = Quaternion.FromToRotation(transform.forward, GetSlopeMoveDirection()) * _characterMoveAmount;
        }
        _rigid.velocity = _characterMoveAmount;
    }
    #endregion
    #region 플레이어 액션
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
    public bool PlayerActionCheck(params PlayerActionType[] types)
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

    public T GetPlayerAction<T>(PlayerActionType type) where T : PlayerAction
    {
        return GetPlayerAction(type) as T;
    }

    public PlayerAction[] GetAllActionsArray()
    {
        return _playerActions.ToArray();
    }

    public PlayerActionType[] GetAllActionTypesArray()
    {
        var Q = from a in _playerActions select a.ActionType;
        return Q.ToArray();
    }

    public void PlayerInteractActionExit()
    {
        PlayerActionExit(PlayerActionType.Interact);
    }
    #endregion
    #region 바닥 체크 및 경사
    private void GroundCheck()
    {
        bool lastGrounded = _isGrounded;
        Vector3 boxCenter = _col.bounds.center;
        Vector3 halfExtents = _col.bounds.extents;
        float maxDistance = 0f;
        if (_playerRenderer.GetHorizontalFlip())
        {
            maxDistance = _col.bounds.extents.y;
            halfExtents.y = _groundCheckRayLength;
        }
        else
        {
            maxDistance = _col.bounds.extents.x;
            halfExtents.y = _groundCheckRayLength;
        }
        _isGrounded = Physics.BoxCast(boxCenter, halfExtents, -transform.up, out _slopeHit, transform.rotation, maxDistance, _groundMask);
        if (lastGrounded == _isGrounded)
            return;
        OnIsGrounded?.Invoke(_isGrounded);
    }

    private bool OnSlope()
    {
        if (_slopeHit.collider != null)
        {
            float angle = Vector3.Angle(transform.up, _slopeHit.normal);
            return angle < playerMovementSO.maxSlopeAngle && angle != 0 && _isGrounded
                 && PlayerActionCheck(PlayerActionType.Jump, PlayerActionType.Dash) == false;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(_moveAmount.normalized, _slopeHit.normal).normalized;
    }
    #endregion
    #region 유틸리티
    public void ForceStop()
    {
        PlayerActionExit(PlayerActionType.Move, PlayerActionType.Dash, PlayerActionType.Jump);
        VeloCityResetImm(true, true);
        _playerInput.InputVectorReset();
        _characterMoveAmount = Vector3.zero;
        _rigid.velocity = Vector3.zero;
        GroundCheck();
        _playerAnimation.FallOrIdleAnimation(IsGrounded);
    }

    public void TrailDisable()
    {
        _playerTrail.TrailDisable();
    }

    public void AfterImageEnable(bool value)
    {
        if (_playerTrail == null)
            return;

        _playerTrail.IsMotionTrail = value;
    }

    public void ColliderSet(PlayerColliderType type)
    {
        Vector3 center = Vector3.zero;
        float height = 0f;
        switch (type)
        {
            case PlayerColliderType.None:
                break;
            case PlayerColliderType.Normal:
                center.y = 0.92f;
                height = 1.9f;
                break;
            case PlayerColliderType.Dash:
                center.y = 0.35f;
                height = 0.8f;
                break;
            default:
                break;
        }
        _col.center = center;
        _col.height = height;
    }
    #endregion

    public void EnableReset()
    {
        for (int i = 0; i < _playerActions.Count; i++)
            _playerActions[i].ActionExit();
        for (int i = 0; i < _enableResetables.Count; i++)
            _enableResetables[i].EnableReset();

        if (_playerRenderer != null)
            _playerRenderer.FlipDirectionChange(DirectionType.Down, true);

        PlayerCameraControll(transform, null);
    }

    public void DisableReset()
    {
        for (int i = 0; i < _disableResetables.Count; i++)
            _disableResetables[i].DisableReset();
    }

    public void WorldCollectionsCountSet(WorldType key, int value)
    {
        WorldCollectionsCountSet(_worldsDatabase.GetWorldData(key), value);
    }

    public void WorldCollectionsCountSet(WorldDataSO key, int value)
    {
        if (_playerJsonData.collectDatas.ContainsKey(key.worldName))
            _playerJsonData.collectDatas[key.worldName] = value;
        SaveJsonData();
    }

    private void LoadJson()
    {
        string path = Application.dataPath + "/Save/JsonData.json";
        string json = File.ReadAllText(path);
        _playerJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<PlayerJsonData>(json);
        if (_playerJsonData == null)
        {
            _playerJsonData = new PlayerJsonData();
        }
        if (_playerJsonData.collectDatas == null)
        {
            _playerJsonData.collectDatas = new Dictionary<string, int>();
            for (int i = 0; i < _worldsDatabase.worldList.Count; i++)
                _playerJsonData.collectDatas.Add(_worldsDatabase.worldList[i].worldName, 0);
        }
        _playerInventory = GetComponent<PlayerInventory>();
        _playerInventory.LoadInventory();
    }

    [ContextMenu("데이터 세이브")]
    public void SaveJsonData()
    {
        string path = Application.dataPath + "/Save/JsonData.json";
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(_playerJsonData);
        File.WriteAllText(path, json);
        _playerInventory.SaveInventory();

#if UNITY_EDITOR
        string debugString = "";
        foreach (var a in _playerJsonData.collectDatas)
            debugString += a.Key + "   " + a.Value + " ";
        Debug.Log("수집품 " + debugString);
#endif
    }

    public void PlayerCameraControll(Transform enable, Transform disable)
    {
        CamManager.Instance.RemoveTargetGroup(disable);
        CamManager.Instance.AddTargetGroup(enable);
    }
}
