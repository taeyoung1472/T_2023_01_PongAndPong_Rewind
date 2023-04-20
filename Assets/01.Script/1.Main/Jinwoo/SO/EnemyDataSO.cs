using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy")]
public class EnemyDataSO : ScriptableObject
{
    public string enemyName;
    public LayerMask targetLayer;
    public float walkSpeed;
    public float runSpeed;

    public float sightRange;
    public float attackRange;

    public float attackDelay;

    public float attackDmg;

    public float health;

    public string info;
}
