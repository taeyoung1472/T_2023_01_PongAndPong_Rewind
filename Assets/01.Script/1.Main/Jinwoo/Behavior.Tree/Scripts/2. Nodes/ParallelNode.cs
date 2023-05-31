using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jinwoo.BehaviorTree
{
    /// <summary> 자식들 리턴에 관계 없이 모두 순회하는 노드 </summary>
    public class ParallelNode : CompositeNode
    {
        public ParallelNode(params INode[] nodes) : base(nodes) { }

        public override INode.NodeState Run()
        {
            foreach (var node in ChildList)
            {
                node.Run();
            }
            return INode.NodeState.Success;
        }
    }
}