using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jinwoo.BehaviorTree
{
    // 조건 참일 경우 Action 수행 및 true 리턴
    // 조건 거짓일 경우 false 리턴
    /// <summary> 조건에 따른 수행 노드 </summary>
    public class IfActionNode : ActionNode
    {
        public Func<INode.NodeState> Condition { get; private set; }

        public IfActionNode(Func<INode.NodeState> condition, Action action)
            : base(action)
        {
            Condition = condition;
        }
        public IfActionNode(ConditionNode condition, ActionNode action)
            : base(action.Action)
        {
            Condition = condition.Condition;
        }

        public override INode.NodeState Run()
        {
            INode.NodeState result = Condition();
            if(result == INode.NodeState.Success) Action();
            return result;
        }
    }
}