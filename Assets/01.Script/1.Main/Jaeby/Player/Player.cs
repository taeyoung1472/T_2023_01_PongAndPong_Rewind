using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMove _playerMove = null;
    private PlayerJump _playerJump = null;
    private PlayerDash _playerDash = null;

    public bool IsGrounded => _playerJump.IsGrounded;

    public bool Moveable { get => _playerMove.Moveable; set => _playerMove.Moveable = value; }
    public bool Jumpable { get => _playerJump.Jumpable; set => _playerJump.Jumpable = value; }
    public bool Dashable { get => _playerDash.Dashable; set => _playerDash.Dashable = value; }

    private void Awake()
    {
        _playerDash = GetComponent<PlayerDash>();
        _playerJump = GetComponent<PlayerJump>();
        _playerMove = GetComponent<PlayerMove>();
    }

    public void PlayerAllActionSet(bool value)
    {
        Moveable = Jumpable = Dashable = value;
    }
}
