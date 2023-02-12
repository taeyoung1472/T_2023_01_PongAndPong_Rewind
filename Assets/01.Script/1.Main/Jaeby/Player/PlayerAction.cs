using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private PlayerActionType _actionType = PlayerActionType.None; // 무슨 액션인가요
    public PlayerActionType ActionType => _actionType;

    protected Player _player = null;

    protected bool _locked = false; // 사용 가능한 액션이니?
    public bool Locked { get => _locked; set => _locked = value; }

    protected bool _excuting = false; // 액션 실행중?
    public bool Excuting => _excuting;

    protected virtual void Start()
    {
        _player = GetComponent<Player>();
    }

    public abstract void ActionExit(); // 액션 강제 종료
}
