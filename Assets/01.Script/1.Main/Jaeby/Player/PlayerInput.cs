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
    [SerializeField]
    private UnityEvent OnWeaponChange = null;

    private Player _player = null;

    private void Awake()
    {
        KeyManager.LoadKey();
    }

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {

        if (_player.PlayerMove.Moveable)
        {
            int x = 0, y = 0;
            if (Input.GetKey(KeyManager.keys[InputType.Right]))
                x++;
            if (Input.GetKey(KeyManager.keys[InputType.Left]))
                x--;
            if (Input.GetKey(KeyManager.keys[InputType.Up]))
                y++;
            if (Input.GetKey(KeyManager.keys[InputType.Down]))
                y--;
            OnMoveInput?.Invoke(new Vector2(x, y));
        }
        if (Input.GetKeyDown(KeyManager.keys[InputType.Jump]))
            OnJumpStart?.Invoke();
        if (Input.GetKeyUp(KeyManager.keys[InputType.Jump]))
            OnJumpEnd?.Invoke();
        if (Input.GetKeyDown(KeyManager.keys[InputType.Dash]))
            OnDash?.Invoke();
        if (Input.GetKeyDown(KeyManager.keys[InputType.Attack]))
            OnAttack?.Invoke();
        if (Input.GetKeyDown(KeyManager.keys[InputType.WeaponChange]))
            OnWeaponChange?.Invoke();
    }
}
