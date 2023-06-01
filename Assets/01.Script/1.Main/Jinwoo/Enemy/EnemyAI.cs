using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jinwoo.BehaviorTree;

using static Jinwoo.BehaviorTree.NodeHelper;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditorInternal;

[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour, ICore
{
    #region 변수들
    public EnemyDataSO _enemyData;

    [Header("PatrolPosition")]
    [SerializeField]
    private Transform[] _patrolPos = null;
    [SerializeField]
    private Transform _currentPatrolPos = null;
    private int _currentPatrolPosIdx;

    [Header("Etc")]
    [SerializeField]
    private Transform eyePos = null;
    [SerializeField]
    protected int _currentHealth = 0;
    private float curidleTime = 0f;

    private bool isHit = false;
    private bool isDie = false;

    private BehaviorTreeRunner _BTRunner = null;
    private Transform _detectedPlayer = null;
    private Vector3? _lastPlayerPos = null;
    private Animator _animator = null;
    private Rigidbody _rigidbody = null;


    const string _attackAnimStateName = "Attack";
    const string _attackAnimTriggerName = "attack";

    const string _walkAnimBoolName = "walk";
    const string _runAnimBoolName = "run";
    const string _dieAnimBoolName = "die";
    const string _hitAnimBoolName = "hit";

    #endregion

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();

        eyePos = transform.Find("EyePos");

        _currentHealth = _enemyData.health;
        isHit = false;
        isDie = false;
        _BTRunner = new BehaviorTreeRunner(MakeNode());
        MakeNode();

    }
    private void Start()
    {
        StartCoroutine(BTUpdate());
    }
    private IEnumerator BTUpdate()
    {
        while (!isDie)
        {
            _BTRunner.Operate();
            yield return new WaitForSeconds(0.05f);
        }
    }

    /// <summary> BT 노드 조립 </summary>
    private INode MakeNode()
    {
        return Selector
            (
                Sequence
                (
                    IfAction(CheckDie, DieAction)
                ),
                Sequence
                (
                    If(CheckHitting),
                    IfAction(CheckHit, HitAction)

                ),
                //공격 시퀀스
                Sequence
                (
                    If(CheckAttacking),
                    IfAction(CheckEnemyWithinAttackRange, DoAttackAction)

                ),
                //추격 시퀀스
                Sequence
                (
                    If(CheckDetectEnemy),
                    IfAction(MoveToDetectEnemy, MoveToDetectEnemyAction)
                ),
                // 패트롤과 아이들
                Selector
                (

                    IfAction(MoveToLastPlayerPositon, MoveToLastPlayerPosAction),
                    Sequence
                    (
                        IfAction(CheckIdleTime, IdleAction)
                    ),
                    Sequence
                    (
                        Action(SetCurPatPos),
                        IfAction(CheckPatrolling, MovePatrolPosAction)
                    )
                    
                )
                
            );
    }

    public void ApplyDamage(int damage)
    {
        _currentHealth -= damage;
        if(_currentHealth > 0)
            isHit = true;
    }
    bool IsAnimationRunning(string stateName)
    {
        if (_animator != null)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            {
                var normalizedTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                return normalizedTime != 0 && normalizedTime < 1f; //아직 애니메이션 진행중
            }
        }
        return false; //애니메이션 끝남
    }

    private bool IsTargetOnSight(Transform target)
    {
        RaycastHit hit;

        var direction = target.position - eyePos.position;

        direction.y = eyePos.forward.y;

        if (Vector3.Angle(direction, eyePos.forward) > _enemyData.fieldOfView * 0.5f)
        {
            //시야각 안에 없다면 false
            return false;
        }

        direction = target.position - eyePos.position;

        if (Physics.Raycast(eyePos.position, direction, out hit, _enemyData._detectRange))
        {
            if (hit.transform == target)
            {
                //플레이어가 시야에 들어옴
                return true;
            }
        }

        return false;
    }

    private void LookTarget(Vector3 dir)
    {
        Quaternion q = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * _enemyData._rotationSpeed);
    }

    private void MoveEnemy(Transform targetPos, bool isWalk)
    {
        

        Vector3 dir = (targetPos.position - transform.position).normalized;
        dir.y = 0;
        Vector3 velocity = new Vector3(dir.x, 0, dir.z);

        if (isWalk)
        {
            velocity *= _enemyData._walkSpeed;

            _animator.SetBool(_walkAnimBoolName, true);
            _animator.SetBool(_runAnimBoolName, false);
        }
        else
        {
            velocity *= _enemyData._runSpeed;

            _animator.SetBool(_walkAnimBoolName, false);
            _animator.SetBool(_runAnimBoolName, true);
        }

        LookTarget(dir);
        _rigidbody.velocity = velocity;
    }
    private void MoveEnemy(Vector3 targetPos, bool isWalk)
    {
        Vector3 dir = (targetPos - transform.position).normalized;
        dir.y = 0;
        Vector3 velocity = new Vector3(dir.x, 0, dir.z);

        if (isWalk)
        {
            velocity *= _enemyData._walkSpeed;

            _animator.SetBool(_walkAnimBoolName, true);
            _animator.SetBool(_runAnimBoolName, false);
        }
        else
        {
            velocity *= _enemyData._runSpeed;

            _animator.SetBool(_walkAnimBoolName, false);
            _animator.SetBool(_runAnimBoolName, true);
        }

        LookTarget(dir);
        _rigidbody.velocity = velocity;
    }
    #region Hit Node

    private INode.NodeState CheckHitting()
    {
        if (IsAnimationRunning("Hit"))
        {
            return INode.NodeState.Running;
        }

        return INode.NodeState.Success;
    }
    private INode.NodeState CheckHit()
    {
        if (isHit == true)
        {
            return INode.NodeState.Success;
        }
        else
        {
            return INode.NodeState.Failure;
        }
    }
    protected virtual void HitAction()
    {
        _rigidbody.velocity = Vector3.zero;
        isHit = false;
        _animator.SetTrigger(_hitAnimBoolName);
    }


    #endregion
    #region Die Node
    private INode.NodeState CheckDie()
    {
        if (_currentHealth <= 0)
        {
            return INode.NodeState.Success;
        }
        else
        {
            return INode.NodeState.Failure;
        }
    }
    protected virtual void DieAction()
    {
        _animator.SetTrigger(_dieAnimBoolName);
        _rigidbody.velocity = Vector3.zero;
        isDie = true;
    }


    #endregion

    #region Attack Node

    protected virtual INode.NodeState CheckAttacking()
    {
        if (IsAnimationRunning(_attackAnimStateName))
        {
            return INode.NodeState.Running;
        }

        return INode.NodeState.Success;
    }
    protected virtual INode.NodeState CheckEnemyWithinAttackRange()
    {
        if (_detectedPlayer != null)
        {
            Vector3 distance = _detectedPlayer.position - transform.position;
            distance.y = 0f;
            if (Vector3.SqrMagnitude(distance) < (_enemyData._meleeAttackRange * _enemyData._meleeAttackRange))
            {
                return INode.NodeState.Success;
            }
        }

        return INode.NodeState.Failure;
    }

    protected virtual void DoAttackAction()
    {
        if (_detectedPlayer != null)
        {
            _animator.SetTrigger(_attackAnimTriggerName);
        }
    }


    #endregion

    #region Detect & Move Node

    private INode.NodeState CheckDetectEnemy()
    {
        var overlapColliders = Physics.OverlapSphere(transform.position, _enemyData._detectRange, _enemyData.targetLayer);
        foreach (var collider in overlapColliders)
        {
            //true : 시야에 타겟이 들어옴 false : 시야에 타겟이 없음
            if (!IsTargetOnSight(collider.transform)) continue;

            var livingEntity = collider.GetComponent<Transform>();

            if (livingEntity != null /*&& !livingEntity.dead*/)
            {
                _detectedPlayer = livingEntity.transform;

                _currentPatrolPos = null;

                return INode.NodeState.Success;
            }
        }
        if(_detectedPlayer != null )
        {
            _lastPlayerPos = _detectedPlayer.position;
        }

        _detectedPlayer = null;
        
        return INode.NodeState.Failure;
    }

    private INode.NodeState MoveToDetectEnemy()
    {
        if (_detectedPlayer != null)
        {
            float distance = Vector3.SqrMagnitude(_detectedPlayer.position - transform.position);
            if (distance < (_enemyData._detectRange * _enemyData._detectRange)) //아직 시야 범위 거리 안에 있음
            {
                return INode.NodeState.Success;
            }
            return INode.NodeState.Running;
        }

        return INode.NodeState.Failure;
    }

    private void MoveToDetectEnemyAction()
    {
        MoveEnemy(_detectedPlayer, false);        
    }

    #endregion

    #region Move LastPlayer Position

    private INode.NodeState MoveToLastPlayerPositon()
    {
        //포지션에 아직 도착 안함
        if (_lastPlayerPos != null && Vector3.SqrMagnitude(_lastPlayerPos.Value - transform.position) > float.Epsilon + 1.2f) //부동소수점 오차 때문에 앱실론 사용
        {
            return INode.NodeState.Success;
        }
        else //원점에 도착
        {
            _animator.SetBool(_runAnimBoolName, false);
            _lastPlayerPos = null;
            return INode.NodeState.Failure;
        }
    }

    private void MoveToLastPlayerPosAction()
    {
        MoveEnemy(_lastPlayerPos.Value, false);
    }

    #endregion


    #region RandomPatrol

    private INode.NodeState CheckPatrolling()
    {
        if (_currentPatrolPos != null)
        {
            float distance = Vector3.SqrMagnitude(_currentPatrolPos.position - transform.position);

            if (distance > (float.Epsilon * float.Epsilon + 1f)) //아직 도착 못함
            {
                return INode.NodeState.Success;
            }
            else //도착
            {
                _animator.SetBool(_walkAnimBoolName, false);
                _currentPatrolPos = null;
                return INode.NodeState.Failure;
            }

        }
        return INode.NodeState.Failure;

    }
    private void SetCurPatPos()
    {
        if (_currentPatrolPos == null)
        {
            int idx = Random.Range(0, _patrolPos.Length);
            if (idx != _currentPatrolPosIdx)
            {
                _currentPatrolPosIdx = idx;
                _currentPatrolPos = _patrolPos[idx];
            }
        }
    }
    private void MovePatrolPosAction()
    {
        MoveEnemy(_currentPatrolPos, true);
    }
    #endregion


    #region Idle & Stop
    private INode.NodeState CheckIdleTime()
    {
        if (curidleTime >= _enemyData.idleTime || _currentPatrolPos != null)
        {
            curidleTime = 0f;
            return INode.NodeState.Failure;
        }
        else
        {
            curidleTime += 0.05f;
            return INode.NodeState.Success;
        }
    }
    private void IdleAction()
    {
        _animator.SetBool(_walkAnimBoolName, false);
        _animator.SetBool(_runAnimBoolName, false);

        //_animator.SetTrigger(_lookAroundAnimTriggerName);

        _rigidbody.velocity = Vector3.zero;
    }

    #endregion

    #region Die


    #endregion

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {

        var leftRayRotation = Quaternion.AngleAxis(-_enemyData.fieldOfView * 0.5f, Vector3.up);
        var leftRayDirection = leftRayRotation * transform.forward;
        Handles.color = new Color(1f, 1f, 1f, 0.2f);
        Handles.DrawSolidArc(eyePos.position, Vector3.up, leftRayDirection, _enemyData.fieldOfView, _enemyData._detectRange);

        Handles.color = new Color(0f, 0f, 1f, 0.2f);
        Handles.DrawSolidArc(eyePos.position, Vector3.up, leftRayDirection, _enemyData.fieldOfView, _enemyData._meleeAttackRange);
    }

#endif
}
