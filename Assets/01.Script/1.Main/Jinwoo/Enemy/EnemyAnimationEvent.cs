using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    const string _attackAnimStateName = "Attack";
    const string _attackAnimTriggerName = "attack";

    const string _walkAnimBoolName = "walk";
    const string _runAnimBoolName = "run";
    const string _dieAnimBoolName = "die";
    const string _hitAnimBoolName = "hit";

    const string _jumpAnimTriggerName = "jump";

    private Animator _animator = null;

    public void Init()
    {
        _animator = GetComponent<Animator>();
    }
}
