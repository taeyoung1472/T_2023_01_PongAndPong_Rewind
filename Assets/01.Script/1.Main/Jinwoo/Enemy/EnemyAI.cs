using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jinwoo.BehaviorTree;

using static Jinwoo.BehaviorTree.NodeHelper;
using UnityEngine.UIElements;
using UnityEditor;

[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour, ICore
{
    [Header("Range")]
    [SerializeField]
    private float _detectRange = 10f;
    [SerializeField]
    private float _meleeAttackRange = 5f;
    [SerializeField]
    private float fieldOfView = 60f;

    [Header("Movement")]
    [SerializeField]
    private float _runSpeed = 10f;
    [SerializeField]
    private float _walkSpeed = 10f;
    [SerializeField]
    private float _rotationSpeed = 20f;

    [Header("PatrolPosition")]
    [SerializeField]
    private Transform[] _patrolPos = null;
    [SerializeField]
    private Transform _currentPatrolPos = null;

    [Header("Etc")]
    [SerializeField]
    private Transform eyePos = null;
    [SerializeField]
    private LayerMask targetLayer; // ���� ��� ���̾�
    [SerializeField]
    private Transform _originPos = null;


    private BehaviorTreeRunner _BTRunner = null;
    private Transform _detectedPlayer = null;
    private Animator _animator = null;
    private Rigidbody _rigidbody = null;

    const string _attackAnimStateName = "Attack";
    const string _attackAnimTriggerName = "attack";

    const string _walkAnimBoolName = "walk";
    const string _runAnimBoolName = "run";

    const string _lookAroundAnimStateName = "LookAround";
    const string _lookAroundAnimTriggerName = "lookaround";
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();

        eyePos = transform.Find("EyePos");

        _BTRunner = new BehaviorTreeRunner(MakeNode());
        MakeNode();

    }
    private void Start()
    {
        StartCoroutine(BTUpdate());
    }
    private IEnumerator BTUpdate()
    {
        while (true)
        {
            _BTRunner.Operate();
            yield return new WaitForSeconds(0.05f);
        }
    }

    /// <summary> BT ��� ���� </summary>
    private INode MakeNode()
    {
        return Selector
            (
                //���� ������
                Sequence
                (
                    If(CheckMeleeAttacking),
                    IfAction(CheckEnemyWithinMeleeAttackRange, DoMeleeAttackAction)

                ),
                //�߰� ������
                Sequence
                (
                    If(CheckDetectEnemy),
                    IfAction(MoveToDetectEnemy, MoveToDetectEnemyAction)
                ),
                Sequence
                (
                    Action(SetCurPatPos),
                    IfAction(CheckPatrolling, MovePatrolPosAction)
                ),

                //���� �������� ���ư��� �ൿ
                IfAction(MoveToOriginPosition, MoveToOriginPositionAction)
            );
    }


    bool IsAnimationRunning(string stateName)
    {
        if (_animator != null)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            {
                var normalizedTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                return normalizedTime != 0 && normalizedTime < 1f; //���� �ִϸ��̼� ������
            }
        }
        return false; //�ִϸ��̼� ����
    }

    private bool IsTargetOnSight(Transform target)
    {
        RaycastHit hit;

        var direction = target.position - eyePos.position;

        direction.y = eyePos.forward.y;

        if (Vector3.Angle(direction, eyePos.forward) > fieldOfView * 0.5f)
        {
            //�þ߰� �ȿ� ���ٸ� false
            return false;
        }

        direction = target.position - eyePos.position;

        if (Physics.Raycast(eyePos.position, direction, out hit, _detectRange))
        {
            if (hit.transform == target)
            {
                //�÷��̾ �þ߿� ����
                return true;
            }
        }

        return false;
    }

    private void LookTarget(Vector3 dir)
    {
        Quaternion q = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * _rotationSpeed);
    }

    private void MoveEnemy(Transform targetPos, bool isWalk)
    {
        

        Vector3 dir = (targetPos.position - transform.position).normalized;
        dir.y = 0;
        Vector3 velocity = new Vector3(dir.x, 0, dir.z);

        if (isWalk)
        {
            velocity *= _walkSpeed;

            _animator.SetBool(_walkAnimBoolName, true);
            _animator.SetBool(_runAnimBoolName, false);
        }
        else
        {
            velocity *= _runSpeed;

            _animator.SetBool(_walkAnimBoolName, false);
            _animator.SetBool(_runAnimBoolName, true);
        }

        LookTarget(dir);
        _rigidbody.velocity = velocity;
    }

    #region Attack Node

    INode.NodeState CheckMeleeAttacking()
    {
        if (IsAnimationRunning(_attackAnimStateName))
        {
            return INode.NodeState.Running;
        }

        return INode.NodeState.Success;
    }
    INode.NodeState CheckEnemyWithinMeleeAttackRange()
    {
        if (_detectedPlayer != null)
        {
            Vector3 distance = _detectedPlayer.position - transform.position;
            distance.y = 0f;
            if (Vector3.SqrMagnitude(distance) < (_meleeAttackRange * _meleeAttackRange))
            {
                return INode.NodeState.Success;
            }
        }

        return INode.NodeState.Failure;
    }

    private void DoMeleeAttackAction()
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
        var overlapColliders = Physics.OverlapSphere(transform.position, _detectRange, targetLayer);
        foreach (var collider in overlapColliders)
        {
            //true : �þ߿� Ÿ���� ���� false : �þ߿� Ÿ���� ����
            if (!IsTargetOnSight(collider.transform)) continue;

            var livingEntity = collider.GetComponent<Transform>();

            if (livingEntity != null /*&& !livingEntity.dead*/)
            {
                _detectedPlayer = livingEntity.transform;

                return INode.NodeState.Success;
            }
        }

        _detectedPlayer = null;

        return INode.NodeState.Failure;
    }

    private INode.NodeState MoveToDetectEnemy()
    {
        if (_detectedPlayer != null)
        {
            float distance = Vector3.SqrMagnitude(_detectedPlayer.position - transform.position);
            if (distance < (_detectRange * _detectRange)) //���� �þ� ���� �Ÿ� �ȿ� ����
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

    #region RandomPatrol

    private INode.NodeState CheckPatrolling()
    {
        if (_currentPatrolPos != null)
        {
            float distance = Vector3.SqrMagnitude(_currentPatrolPos.position - transform.position);
            
            if (distance > (float.Epsilon * float.Epsilon + 1f)) //���� ���� ����
            {
                Debug.Log("Patrolling");
                return INode.NodeState.Success;
            }
            else //����
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
        if(_currentPatrolPos == null)
        {
            int idx = Random.Range(0, _patrolPos.Length);
            _currentPatrolPos = _patrolPos[idx];

        }
    }
    private void MovePatrolPosAction()
    {
        MoveEnemy(_currentPatrolPos, true);
    }
    #endregion

    #region Move Origin Pos Node

    private INode.NodeState MoveToOriginPosition()
    {
        //������ ���� ���� ����
        if (Vector3.SqrMagnitude(_originPos.position - transform.position) > float.Epsilon * float.Epsilon + 1f) //�ε��Ҽ��� ���� ������ �۽Ƿ� ���
        {
            return INode.NodeState.Success;
        }
        else //������ ����
        {
            _animator.SetBool(_walkAnimBoolName, false);

            return INode.NodeState.Failure;
        }
    }

    private void MoveToOriginPositionAction()
    {
        MoveEnemy(_originPos, true);
    }

    #endregion

    #region Idle & Stop

    INode.NodeState CheckLookingAround()
    {
        if (IsAnimationRunning(_lookAroundAnimStateName))
        {
            return INode.NodeState.Running;
        }

        return INode.NodeState.Success;
    }
    private void IdleAction()
    {
        _animator.SetBool(_walkAnimBoolName, false);
        _animator.SetBool(_runAnimBoolName, false);

        _animator.SetTrigger(_lookAroundAnimTriggerName);

        _rigidbody.velocity = Vector3.zero;
    }

    #endregion

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {

        var leftRayRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
        var leftRayDirection = leftRayRotation * transform.forward;
        Handles.color = new Color(1f, 1f, 1f, 0.2f);
        Handles.DrawSolidArc(eyePos.position, Vector3.up, leftRayDirection, fieldOfView, _detectRange);

        Handles.color = new Color(0f, 0f, 1f, 0.2f);
        Handles.DrawSolidArc(eyePos.position, Vector3.up, leftRayDirection, fieldOfView, _meleeAttackRange);
    }

#endif
}
