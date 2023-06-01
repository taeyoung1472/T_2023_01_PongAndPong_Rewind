using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jinwoo.BehaviorTree;

using static Jinwoo.BehaviorTree.NodeHelper;
using UnityEditor;

public class FarEnemy : EnemyAI
{
    [SerializeField]
    private Transform shootPos;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void DoAttackAction()
    {
        base.DoAttackAction();
    }
}
