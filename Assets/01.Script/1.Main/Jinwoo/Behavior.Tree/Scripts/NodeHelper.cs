using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jinwoo.BehaviorTree
{
    // Core에서
    // using static Jinwoo.BehaviorTree.NodeHelper;
    public static class NodeHelper
    {
        public static SelectorNode Selector(params INode[] nodes) => new SelectorNode(nodes);
        public static SequenceNode Sequence(params INode[] nodes) => new SequenceNode(nodes);
        public static ParallelNode Parallel(params INode[] nodes) => new ParallelNode(nodes);

        public static ConditionNode Condition(Func<INode.NodeState> condition) => new ConditionNode(condition);
        public static ConditionNode If(Func<INode.NodeState> condition) => new ConditionNode(condition);
        public static ActionNode Action(Action action) => new ActionNode(action);

        public static IfActionNode IfAction(Func<INode.NodeState> condition, Action action)
            => new IfActionNode(condition, action);
        public static IfElseActionNode IfElseAction(Func<INode.NodeState> condition, Action ifAction, Action ifElseAction)
            => new IfElseActionNode(condition, ifAction, ifElseAction);
    }
}