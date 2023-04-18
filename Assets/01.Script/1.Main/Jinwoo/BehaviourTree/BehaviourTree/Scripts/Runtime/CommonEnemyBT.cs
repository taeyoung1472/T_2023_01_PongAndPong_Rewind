using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeEditorDev;

public class CommonEnemyBT : MonoBehaviour
{
    public BehaviourTree tree;

    [SerializeField] private EnemyAI enemy;
    public EnemyAI Enemy
    {
        get => enemy;
        set => enemy = value;
    }

    // ���� ��ü ���� �ý����� �����ϴ� ���丮�� �����̳� ��ü
    Context context;

    void Start()
    {
        context = CreateBehaviourTreeContext();
        tree = tree.Clone();
        tree.Bind(context);
    }

    void Update()
    {
        if (tree)
        {
            tree.Update();
        }
    }

    Context CreateBehaviourTreeContext()
    {
        return Context.CreateEnemyFromGameObject(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (!tree)
        {
            return;
        }

        BehaviourTree.Traverse(tree.rootNode, (n) => {
            if (n.drawGizmos)
            {
                n.OnDrawGizmos();
            }
        });
    }
}
