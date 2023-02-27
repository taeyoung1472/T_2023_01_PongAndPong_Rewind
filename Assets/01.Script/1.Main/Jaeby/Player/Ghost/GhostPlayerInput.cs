using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GhostPlayerInput : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<Vector2> OnMovementInput = null;

    private void Update()
    {
        OnMovementInput?.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
    }
}
