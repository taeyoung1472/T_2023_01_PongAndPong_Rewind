using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jinwoo.BehaviorTree
{
    /// <summary> 행동 수행 노드 </summary>
    public class ActionNode : ILeafNode
    {
        public Action Action { get; protected set; }
        public ActionNode(Action action)
        {
            Action = action;
        }

        public virtual INode.NodeState Run()
        {
            Action();
            return INode.NodeState.Success;
        }

        // Action <=> ActionNode 타입 캐스팅
        public static implicit operator ActionNode(Action action) => new ActionNode(action);
        public static implicit operator Action(ActionNode action) => new Action(action.Action);
    }
}