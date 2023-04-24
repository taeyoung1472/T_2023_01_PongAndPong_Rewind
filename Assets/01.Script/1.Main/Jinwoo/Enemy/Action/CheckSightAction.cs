using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeEditorDev;
public class CheckSightAction : ActionNode
{
    private float range;
    public Transform target;
    
    protected override void OnStart()
    {
        range = blackboard.enemyData.sightRange;
        //target = StageManager.Instance.GetCurrentPlayer().transform;
        target = context.enemyAI.target.transform;
        blackboard.target = target.gameObject;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        //Debug.Log(target.name);
        //float distance = Vector3.Distance(target.position, origin.position); 
        Debug.Log("시야 체크");
        if (IsTargetOnSight(target))
        {
            
            context.animator.SetBool("IsWalk", false);
            context.animator.SetBool("IsRun", true);
            return State.Success;
        }
        else
        {
            context.animator.SetBool("IsRun", false);
            return State.Failure;
        }
    }
    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = target.position - context.enemyAI.eyeTransform.position;
        Gizmos.DrawRay(context.enemyAI.eyeTransform.position, direction);
    }

   private bool IsTargetOnSight(Transform target)
    {
        RaycastHit hit;

        Vector3 direction = context.enemyAI.eyeTransform.forward;

        //direction.y = context.enemyAI.eyeTransform.forward.y;

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
