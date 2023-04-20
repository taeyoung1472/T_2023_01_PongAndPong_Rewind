using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeEditorDev;
public class CheckPoint : ActionNode
{


    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.agent.pathPending)
        {
            return State.Running;
        }

        if (context.agent.remainingDistance < 1.0f)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }

        //if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        //{

        //}

    }

}
