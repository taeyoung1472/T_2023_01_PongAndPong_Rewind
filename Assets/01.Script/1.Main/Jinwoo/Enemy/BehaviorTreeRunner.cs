using Jinwoo.BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BehaviorTreeRunner : MonoBehaviour
{
    INode _rootNode;
    public BehaviorTreeRunner(INode rootNode)
    {
        _rootNode = rootNode;
    }

    public void Operate()
    {
        _rootNode.Run();
    }
}
