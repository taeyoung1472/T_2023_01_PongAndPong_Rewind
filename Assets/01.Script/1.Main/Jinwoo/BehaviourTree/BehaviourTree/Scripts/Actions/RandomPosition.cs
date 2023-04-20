using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeEditorDev;

[System.Serializable]
public class RandomPosition : ActionNode {
    //public Vector2 min = Vector2.one * -10;
    //public Vector2 max = Vector2.one * 10;

    public int curPatrolPointIdx = 0;


    public float speed = 3;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;
    protected override void OnStart() {
        
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        //Vector3 pos = new Vector3();
        //pos.x = Random.Range(min.x, max.x);
        //pos.y = Random.Range(min.y, max.y);
        //blackboard.moveToPosition = pos;
        //return State.Success;


        blackboard.patrolPoint = context.enemyAI.patrolPoint[curPatrolPointIdx++ % 2];

        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.destination = blackboard.patrolPoint.position;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;

        context.animator.SetBool("IsWalk", true);

        return State.Success;
    }
}
