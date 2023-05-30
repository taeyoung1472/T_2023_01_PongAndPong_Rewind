using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jinwoo.BehaviorTree
{
    /// <summary> 행동트리 최상위 인터페이스 </summary>
    public interface INode
    {
        public enum NodeState
        {
            Running,
            Success,
            Failure,
        }

        public NodeState Run();
    }
}