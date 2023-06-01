using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jinwoo.BehaviorTree
{
    /// <summary> �ൿƮ�� �ֻ��� �������̽� </summary>
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