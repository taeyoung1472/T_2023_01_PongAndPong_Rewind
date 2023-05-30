using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jinwoo.BehaviorTree
{
    // 조건 참일 경우 IfAction 수행 및 true 리턴
    // 조건 거짓일 경우 ElseAction 수행 및 false 리턴
    /// <summary> 조건에 따른 수행 노드 </summary>
    public class IfElseActionNode : ILeafNode
    {
        public Func<INode.NodeState> Condition { get; private set; }
        public Action IfAction { get; private set; }
        public Action ElseAction { get; private set; }

        public IfElseActionNode(Func<INode.NodeState> condition, Action ifAction, Action elseAction)
        {
            Condition = condition;
            IfAction = ifAction;
            ElseAction = elseAction;
        }

        public IfElseActionNode(ConditionNode condition, ActionNode ifAction, ActionNode elseAction)
        {
            Condition = condition.Condition;
            IfAction = ifAction.Action;
            ElseAction = elseAction.Action;
        }

        public INode.NodeState Run()
        {
            INode.NodeState result = Condition();

            if (result == INode.NodeState.Success) IfAction();
            else ElseAction();

            return result;
        }
    }
}