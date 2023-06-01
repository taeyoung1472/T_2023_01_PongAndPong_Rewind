using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy")]
public class EnemyDataSO : ScriptableObject
{
    public string enemyName;

    [Header("Range")]
    public float _detectRange = 6f;
    public float _meleeAttackRange = 2f;
    public float fieldOfView = 180f;

    [Header("Movement")]
    public float _runSpeed = 5f;
    public float _walkSpeed = 3f;
    public float _rotationSpeed = 40f;

    [Header("Health")]
    public int health;

    public LayerMask targetLayer; // 추적 대상 레이어

    public float idleTime = 1.5f;

    [TextArea]
    public string info;
}
