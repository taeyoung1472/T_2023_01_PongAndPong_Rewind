using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    #region 변수들
    //[SerializeField] private EnemyDataSO enemyData;
    //public EnemyDataSO EnemyData
    //{
    //    get => enemyData;
    //    set => enemyData = value;
    //}

    protected float health;// 현재 체력
    public float Health { get => health; set => health = value; } // 현재 체력
    protected bool dead = false; // 사망 상태
    public bool Dead { get => dead; set => dead = value; } // 사망 상태

    public float lastAttackTime = 0;

    protected LayerMask whatIsTarget; // 추적 대상 레이어
    protected Transform attackRoot;

    public GameObject target;
    protected bool isAttack = false;

    public bool IsAttack { get { return isAttack; } set { isAttack = value; } }

    protected const float minTimeBetDamaged = 0.1f;
    protected float lastDamagedTime;


    public Transform eyeTransform;
    public Transform[] patrolPoint;
    // 잠깐 무적 시간
    protected bool IsInvulnerable
    {
        get
        {
            if (Time.time >= lastDamagedTime + minTimeBetDamaged) return false;

            return true;
        }
    }
    #endregion
    public void SetUp(float health)
    {
        this.health = health;
    }
    public bool EnemyDie()
    {
        if (health <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawSphere(transform.position, 7f);
        var leftRayRotation = Quaternion.AngleAxis(-60f * 0.5f, Vector3.up);
        var leftRayDirection = leftRayRotation * transform.forward;
        Handles.color = new Color(1f, 1f, 1f, 0.2f);
        Handles.DrawSolidArc(eyeTransform.position, Vector3.up, leftRayDirection, 60f, 7f);
    }

#endif

}
