using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 날짜 : 2021-01-16 PM 10:20:49
// 작성자 : Rito

namespace Jinwoo.BehaviorTree
{
    /// <summary> 조건에 따른 Composite 수행 노드 </summary>
    public abstract class DecoratedCompositeNode : CompositeNode
    {
        public Func<INode.NodeState> Condition { get; protected set; }

        public CompositeNode Composite { get; protected set; }

        public DecoratedCompositeNode(Func<INode.NodeState> condition, CompositeNode composite)
        {
            Condition = condition;
            Composite = composite;
        }

        public override INode.NodeState Run()
        {
            if (Condition() == INode.NodeState.Failure)
                return INode.NodeState.Failure;

            return Composite.Run();
        }
    }
}