using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTreeEditorDev {

    // 컨텍스트는 모든 노드가 액세스할 수 있는 공유 객체임
    // 일반적으로 사용되는 컴포넌트 및 하위 시스템은 여기에 저장해야 함
    // 여기에 무엇을 추가해야 하는지는 게임에 따라 다름.
    // 자유롭게 이 클래스를 확장
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

        // 여기에 다른 게임별 시스템 추가

        public static Context CreateFromGameObject(GameObject gameObject) {
            // 일반적으로 사용되는 모든 컴포넌트 가져오기
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

            

            // 여기에 필요한 다른 것을 추가 ...

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