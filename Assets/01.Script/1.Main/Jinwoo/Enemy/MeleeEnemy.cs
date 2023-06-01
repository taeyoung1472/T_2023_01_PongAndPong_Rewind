using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jinwoo.BehaviorTree;

using static Jinwoo.BehaviorTree.NodeHelper;
using UnityEditor;

public class MeleeEnemy : EnemyAI
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            ApplyDamage(40);
        }
    }

}
