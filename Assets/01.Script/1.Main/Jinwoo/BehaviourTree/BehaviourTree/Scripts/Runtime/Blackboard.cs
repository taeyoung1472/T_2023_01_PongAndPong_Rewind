using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeEditorDev {

    // 모든 노드가 공유하는 블랙보드 컨테이너
    // 여러 노드가 읽기 및 쓰기 액세스가 필요한 임시 데이터를 저장하는 데 사용
    // 특정 사용 사례에 적합한 다른 속성을 여기에 추가
    [System.Serializable]
    public class Blackboard {

        public Vector3 moveToPosition;
        public EnemyDataSO enemyData;
        public GameObject target;
    }
}