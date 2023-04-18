using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTreeEditorDev {

    // ���ؽ�Ʈ�� ��� ��尡 �׼����� �� �ִ� ���� ��ü��
    // �Ϲ������� ���Ǵ� ������Ʈ �� ���� �ý����� ���⿡ �����ؾ� ��
    // ���⿡ ������ �߰��ؾ� �ϴ����� ���ӿ� ���� �ٸ�.
    // �����Ӱ� �� Ŭ������ Ȯ��
    public class Context {
        public GameObject gameObject;
        public Transform transform;
        public Animator animator;
        public Rigidbody physics;
        public NavMeshAgent agent;
        public SphereCollider sphereCollider;
        public BoxCollider boxCollider;
        public CapsuleCollider capsuleCollider;
        public CharacterController characterController;

        public EnemyAI enemyAI;

        // ���⿡ �ٸ� ���Ӻ� �ý��� �߰�

        public static Context CreateFromGameObject(GameObject gameObject) {
            // �Ϲ������� ���Ǵ� ��� ������Ʈ ��������
            Context context = new Context();
            context.gameObject = gameObject;
            context.transform = gameObject.transform;
            context.animator = gameObject.GetComponent<Animator>();
            context.physics = gameObject.GetComponent<Rigidbody>();
            context.agent = gameObject.GetComponent<NavMeshAgent>();
            context.sphereCollider = gameObject.GetComponent<SphereCollider>();
            context.boxCollider = gameObject.GetComponent<BoxCollider>();
            context.capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
            context.characterController = gameObject.GetComponent<CharacterController>();

            

            // ���⿡ �ʿ��� �ٸ� ���� �߰� ...

            return context;
        }
        public static Context CreateEnemyFromGameObject(GameObject gameObject)
        {
            Context context = new Context();

            context.gameObject = gameObject;
            context.transform = gameObject.transform;
            context.animator = gameObject.transform.Find("Model").GetComponent<Animator>();
            context.physics = gameObject.GetComponent<Rigidbody>();
            context.agent = gameObject.GetComponent<NavMeshAgent>();
            context.capsuleCollider = gameObject.GetComponent<CapsuleCollider>();

            context.enemyAI = gameObject.GetComponent<EnemyAI>();


            return context;
        }
    }
}