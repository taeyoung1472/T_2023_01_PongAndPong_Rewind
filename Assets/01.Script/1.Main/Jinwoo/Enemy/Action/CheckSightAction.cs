using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeEditorDev;
public class CheckSightAction : ActionNode
{
    private float range;
    private Transform target;
    private Transform origin;
    protected override void OnStart()
    {
        range = context.enemyAI.EnemyData.sightRange;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        float distance = Vector3.Distance(target.position, origin.position);
        return distance <= range ? State.Success : State.Failure;
    }

}
