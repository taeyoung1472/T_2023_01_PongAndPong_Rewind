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

    protected float health;// ���� ü��
    protected bool dead = false; // ��� ����
    public float Health { get => health; set => health = value; } // ���� ü��
    public bool Dead { get => dead; set => dead = value; } // ��� ����

    public float lastAttackTime = 0;

    protected GameObject target;
    public GameObject Target
    {
        get => target;
        set => target = value;
    }

    protected LayerMask whatIsTarget; // ���� ��� ���̾�
    protected Transform attackRoot;

    protected bool isAttack = false;

    public bool IsAttack { get { return isAttack; } set { isAttack = value; } }

    protected const float minTimeBetDamaged = 0.1f;
    protected float lastDamagedTime;

    // ��� ���� �ð�
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
