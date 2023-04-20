using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeEditorDev;
public class CheckAttackRangeAction : ActionNode
{
    private float range;
    public Transform target;
    protected override void OnStart()
    {
        range = blackboard.enemyData.attackRange;
        //target = StageManager.Instance.GetCurrentPlayer().transform;
        target = context.enemyAI.target.transform;
        blackboard.target = target.gameObject;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {

        //float distance = Vector3.Distance(target.position, origin.position); 
        if (IsTargetOnSight(target))
        {
            Debug.Log("����!!!");
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
    private bool IsTargetOnSight(Transform target)
    {
        RaycastHit hit;

        Vector3 direction = target.position - context.enemyAI.eyeTransform.position;



        if (Physics.Raycast(context.enemyAI.eyeTransform.position, direction, out hit, range))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                return true;
            }
        }

        return false;
    }

}
