using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeEditorDev;

public class PatrolAction : ActionNode
{
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;

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
            Debug.Log("�� �Ӵ�");
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance)
        {
            Debug.Log("���� ����Ʈ ����!!!"); 
            context.animator.SetBool("IsWalk", false);
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("����!!!");
            return State.Failure;
        }

        Debug.Log("��Ʈ�� ����");
        return State.Failure;
    }

}
