using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeEditorDev;

public class MeleeAttackAction : ActionNode
{
    protected override void OnStart()
    {
        context.agent.isStopped = true;
    }

    protected override void OnStop()
    {
        context.agent.isStopped = false;

    }

    protected override State OnUpdate()
    {
        context.animator.SetTrigger("IsAttack");
        return State.Success;
    }

}
