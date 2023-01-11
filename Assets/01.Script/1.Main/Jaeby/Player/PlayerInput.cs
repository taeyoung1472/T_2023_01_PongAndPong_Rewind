using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<Vector2> OnMoveInput = null;
    [SerializeField]
    private UnityEvent OnJumpStart = null;
    [SerializeField]
    private UnityEvent OnJumpEnd = null;
    [SerializeField]
    private UnityEvent OnDash = null;
    [SerializeField]
    private UnityEvent OnAttack = null;

    private Player _player = null;

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if(_player.Moveable)
            OnMoveInput?.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        if (Input.GetKeyDown(KeyCode.Space))
            OnJumpStart?.Invoke();
        if (Input.GetKeyUp(KeyCode.Space))
            OnJumpEnd?.Invoke();
        if (Input.GetMouseButtonDown(1))
            OnDash?.Invoke();
        if (Input.GetMouseButtonDown(0))
            OnAttack?.Invoke();
    }

}
