using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VirtualPlayerInput : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<Vector2> OnMoveInput = null;
    [SerializeField]
    private UnityEvent OnJumpStart = null;
    [SerializeField]
    private UnityEvent OnDash = null;
    [SerializeField]
    private UnityEvent OnAttack = null;

    private Player _player = null;

    Vector2 virtualInput;

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (_player.PlayerMove.Moveable)
            OnMoveInput?.Invoke(virtualInput);
    }

    public void MoveLeft()
    {
        virtualInput = Vector2.left;
    }

    public void MoveRight()
    {
        virtualInput = Vector2.right;
    }

    public void Stop()
    {
        virtualInput = Vector2.zero;
    }

    public void Attack()
    {
        OnAttack?.Invoke();
    }

    public void Dash()
    {
        OnDash?.Invoke();
    }

    public void Jump()
    {
        OnJumpStart?.Invoke();
    }
}
