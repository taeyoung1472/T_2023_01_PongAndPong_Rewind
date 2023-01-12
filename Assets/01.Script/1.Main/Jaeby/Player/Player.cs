using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 캐싱 준비
    private PlayerMove _playerMove = null;
    private PlayerJump _playerJump = null;
    private PlayerDash _playerDash = null;
    private PlayerAttack _playerAttack = null;
    private PlayerWallGrab _playerWallGrab = null;
    private PlayerRenderer _playerRenderer = null;
    private PlayerAnimation _playerAnimation = null;
    private GravityModule _gravityModule = null;

    // 프로퍼티
    public PlayerMove PlayerMove => _playerMove;
    public PlayerJump PlayerJump => _playerJump;
    public PlayerDash PlayerDash => _playerDash;
    public PlayerAttack PlayerAttack => _playerAttack;
    public PlayerWallGrab PlayerWallGrab => _playerWallGrab;
    public PlayerRenderer PlayerRenderer => _playerRenderer;
    public PlayerAnimation PlayerAnimation => _playerAnimation;
    public GravityModule GravityModule => _gravityModule;

    private void Awake()
    {
        _playerDash = GetComponent<PlayerDash>();
        _playerJump = GetComponent<PlayerJump>();
        _playerMove = GetComponent<PlayerMove>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerWallGrab = GetComponent<PlayerWallGrab>();
        _playerRenderer = transform.Find("AgentRenderer").GetComponent<PlayerRenderer>();
        _playerAnimation = _playerRenderer.GetComponent<PlayerAnimation>();
        _gravityModule = GetComponent<GravityModule>();
    }

    /// <summary>
    /// 플레이어 상태 모두 변경
    /// </summary>
    /// <param name="value"></param>
    public void PlayerAllActionSet(bool value)
    {
        _playerAttack.Attackable = _playerMove.Moveable = _playerJump.Jumpable = _playerDash.Dashable = value;
    }
}
