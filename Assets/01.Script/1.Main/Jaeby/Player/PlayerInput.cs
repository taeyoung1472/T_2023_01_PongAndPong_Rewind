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

    private void Update()
    {
        OnMoveInput?.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        if (Input.GetKeyDown(KeyCode.W))
            OnJumpStart?.Invoke();
        if (Input.GetKeyUp(KeyCode.W))
            OnJumpEnd?.Invoke();
        if (Input.GetKeyDown(KeyCode.LeftShift))
            OnDash?.Invoke();
    }

}
