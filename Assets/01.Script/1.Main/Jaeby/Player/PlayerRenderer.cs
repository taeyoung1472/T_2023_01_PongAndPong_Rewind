using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerRenderer : MonoBehaviour
{
    private bool _fliped = false;

    [SerializeField]
    private UnityEvent<bool> OnFliped = null;

    public void Flip(Vector2 input)
    {
        Vector3 local = transform.localScale;
        if (_fliped) // ¿ÞÂÊ
        {
            if(input.x > 0f)
            {
                local.x *= -1f;
                _fliped = false;
            }
        }
        else
        {
            if(input.x < 0f)
            {
                local.x *= -1f;
                _fliped = true;
            }
        }
        transform.localScale = local;
        OnFliped?.Invoke(_fliped);
    }

    public void ForceFlip()
    {
        if (_fliped)
            Flip(Vector2.right);
        else
            Flip(Vector2.left);
    }
}
