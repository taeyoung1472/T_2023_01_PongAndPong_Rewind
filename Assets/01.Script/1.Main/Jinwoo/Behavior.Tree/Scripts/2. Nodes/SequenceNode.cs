using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jinwoo.BehaviorTree
{
    /// <summary> 자식들 중 false가 나올 때까지 연속으로 순회하는 노드 </summary>
    public class SequenceNode : CompositeNode
    {
        public SequenceNode(params INode[] nodes) : base(nodes) { }

        public override INode.NodeState Run()
        {
            if (ChildList == null || ChildList.Count == 0)
                return INode.NodeState.Failure;

            foreach (var node in ChildList)
            {
                INode.NodeState result = node.Run();

                switch(result)
                {
                    case INode.NodeState.Running:
                        return INode.NodeState.Running;
                    case INode.NodeState.Success:
                        continue;
                    case INode.NodeState.Failure:
                        return INode.NodeState.Failure;
                }
            }
            return INode.NodeState.Success;
        }
    }
}