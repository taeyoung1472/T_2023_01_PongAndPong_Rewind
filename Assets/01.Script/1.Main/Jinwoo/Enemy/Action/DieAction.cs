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
        if(context.enemyAI.EnemyDie()) //利 贸第咙
        {
            return State.Success;
        }
        else //救 第脸澜
        {
            return State.Failure;
        }
    }
}
