using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeEditorDev {

    // ��� ��尡 �����ϴ� ������ �����̳�
    // ���� ��尡 �б� �� ���� �׼����� �ʿ��� �ӽ� �����͸� �����ϴ� �� ���
    // Ư�� ��� ��ʿ� ������ �ٸ� �Ӽ��� ���⿡ �߰�
    [System.Serializable]
    public class Blackboard {

        public Vector3 moveToPosition;
        public EnemyDataSO enemyData;
        public GameObject target;
        public Transform patrolPoint;

        public bool isPatrol;
        public bool isChasing;
        public bool isAttacking;
    }
}