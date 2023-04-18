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
        if(context.enemyAI.EnemyDie()) //�� ó����
        {
            return State.Success;
        }
        else //�� ������
        {
            return State.Failure;
        }
    }
}
