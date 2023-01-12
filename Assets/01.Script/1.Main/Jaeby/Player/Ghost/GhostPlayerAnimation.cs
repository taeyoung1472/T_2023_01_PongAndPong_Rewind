using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlayerAnimation : MonoBehaviour
{
    private Animator _animator = null;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void MoveAnimationSet(Vector2 input)
    {
        _animator.SetInteger("MoveX", (int)input.x);
        _animator.SetInteger("MoveY", (int)input.y);
    }
}
