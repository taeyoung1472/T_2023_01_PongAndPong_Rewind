using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jinwoo.BehaviorTree
{
    /// <summary> 자식들을 순회하며 true인 것 하나만 수행하는 노드 </summary>
    public class SelectorNode : CompositeNode
    {
        public SelectorNode(params INode[] nodes) : base(nodes) { }

        public override INode.NodeState Run()
        {
            if (ChildList == null)
                return INode.NodeState.Failure;

            foreach (var node in ChildList)
            {
                INode.NodeState result = node.Run();
                switch(result)
                {
                    case INode.NodeState.Running:
                        return INode.NodeState.Running;
                    case INode.NodeState.Success:
                        return INode.NodeState.Success;
                }
            }
            return INode.NodeState.Failure;
        }
    }
}