using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyDataSO enemyData;
    public EnemyDataSO EnemyData
    {
        get => enemyData;
        set => enemyData = value;
    }

    protected float health;// 현재 체력
    protected bool dead = false; // 사망 상태
    public float Health { get => health; set => health = value; } // 현재 체력
    public bool Dead { get => dead; set => dead = value; } // 사망 상태

    public float lastAttackTime = 0;

    protected GameObject target;
    public GameObject Target
    {
        get => target;
        set => target = value;
    }

    protected LayerMask whatIsTarget; // 추적 대상 레이어
    protected Transform attackRoot;

    protected bool isAttack = false;

    public bool IsAttack { get { return isAttack; } set { isAttack = value; } }

    protected const float minTimeBetDamaged = 0.1f;
    protected float lastDamagedTime;

    // 잠깐 무적 시간
    protected bool IsInvulnerable
    {
        get
        {
            if (Time.time >= lastDamagedTime + minTimeBetDamaged) return false;

            return true;
        }
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
}
