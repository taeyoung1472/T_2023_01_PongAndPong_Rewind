using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jinwoo.BehaviorTree;

using static Jinwoo.BehaviorTree.NodeHelper;

public class CoreTest : MonoBehaviour, ICore
{
    private INode _rootNode;

    private void Awake()
    {
        MakeNode();
    }

    private void Update()
    {
        _rootNode.Run();
    }

    /// <summary> BT 노드 조립 </summary>
    private void MakeNode()
    {
        _rootNode =

            //If(() => Input.GetKey(KeyCode.Q)).
            Selector
            (
                IfAction  (KeyMoveInput, KeyMoveAction),
                IfAction(MouseMoveInput, MouseMoveAction)
            );
    }

    private INode.NodeState KeyMoveInput()
    {
        bool result =
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D);

        Debug.Log($"Condition : Key Move INPUT ({result})");
        return INode.NodeState.Success;
    }
    private INode.NodeState MouseMoveInput()
    {
        bool result = Input.GetMouseButton(1);
        Debug.Log($"Condition : Mouse Move INPUT ({result})");
        return INode.NodeState.Success;
    }
    private void KeyMoveAction() => Debug.Log($"Action : Key Move");
    private void MouseMoveAction() => Debug.Log($"Action : Mouse Move");


    #region Attack Node
    
    private INode.NodeState CheckAttacking()
    {
        return INode.NodeState.Success;
    }


    #endregion

    #region Detect & Move Node

    private INode.NodeState CheckDetectEnemy()
    {

        return INode.NodeState.Failure;
    }

    private INode.NodeState MoveToDetectEnemy()
    {
        return INode.NodeState.Failure;
    }


    #endregion

    #region Move Origin Pos Node

    private INode.NodeState MoveToOriginPosition()
    {

        return INode.NodeState.Running;
    }


    #endregion 


    private void OnDrawGizmos()
    {
        
    }



}
