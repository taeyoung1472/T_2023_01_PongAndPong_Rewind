using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeEditorDev;

public class DieAction : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if(context.enemyAI.EnemyDie()) //적 처뒤짐
        {
            return State.Success;
        }
        else //안 뒤졌음
        {
            return State.Failure;
        }
    }
}
