using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private PlayerActionType _actionType = PlayerActionType.None; // ���� �׼��ΰ���
    public PlayerActionType ActionType => _actionType;

    protected Player _player = null;

    protected bool _locked = false; // ��� ������ �׼��̴�?
    public bool Locked { get => _locked; set => _locked = value; }

    protected bool _excuting = false; // �׼� ������?
    public bool Excuting => _excuting;

    protected virtual void Start()
    {
        _player = GetComponent<Player>();
    }

    public abstract void ActionExit(); // �׼� ���� ����
}
