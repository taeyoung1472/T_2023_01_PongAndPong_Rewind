using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private PlayerAttackSO _playerAttackSO = null;

    private Animator _animator = null;
    private Rigidbody _rigid = null;
    private bool _isGrounded = false;

    private Coroutine _attackAniCorou = null;

    [SerializeField]
    private UnityEvent OnAttackStarted = null;
    [SerializeField]
    private UnityEvent OnAttackEnded = null;

    private Player _player = null;

    private void Start()
    {
        _player = GetComponentInParent<Player>();
        _rigid = GetComponentInParent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    public void MoveAnimation(Vector2 input)
    {
        _animator.SetBool("Move", Mathf.Abs(input.x) > 0f);
    }

    public void DashAnimation(Vector2 input)
    {
        if (input.y > 0f)
            _animator.Play("PlayerDashJump");
        else if (input.y < 0f)
            _animator.Play("PlayerFall");
        else
            _animator.Play("PlayerDash");
        _animator.Update(0);
    }

    public void JumpAnimation()
    {
        _animator.Play("PlayerJump");
        _animator.Update(0);
    }

    public void WallGrabAnimation(bool val)
    {
        if (val == false)
            return;
        _animator.SetTrigger("AttackForceExit");
        _animator.Update(0);
        _animator.Play("PlayerWallGrab");
        _animator.Update(0);
    }

    public void IdleAnimation()
    {
        _animator.Play("PlayerIdle");
        _animator.Update(0);
    }

    public void FallOrIdleAnimation(bool isGround)
    {
        _isGrounded = isGround;
        if (_isGrounded)
        {
            IdleAnimation();
        }
        else
        {
            _animator.Play("PlayerFall");
            _animator.Update(0);
        }
    }

    public void AnimationGroundCheck(bool value)
    {
        _animator.SetBool("IsGround", value);
    }

    public void MeleeAttackAnimation(int index)
    {
        string name = $"PlayerMeleeAttack{index}";
        _animator.Play(name);
        _animator.Update(0);
        if (_attackAniCorou != null)
            StopCoroutine(_attackAniCorou);
        _attackAniCorou = StartCoroutine(AttackAnimationEndWaitCoroutine(name, AttackState.Melee));
    }

    public void RangeAttackAnimation()
    {
        _animator.Play("PlayerRangeAttack");
        _animator.Update(0);
        if (_attackAniCorou != null)
            StopCoroutine(_attackAniCorou);
        _attackAniCorou = StartCoroutine(AttackAnimationEndWaitCoroutine("PlayerRangeAttack", AttackState.Range));
    }

    private IEnumerator AttackAnimationEndWaitCoroutine(string aniName, AttackState attackState)
    {
        OnAttackStarted?.Invoke();
        //yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(1).IsName(aniName) == false);
        yield return new WaitUntil(() => 
        (
        _animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= (attackState == AttackState.Melee ? _playerAttackSO.meleeAttackDelay : _playerAttackSO.rangeAttackDelay)) || 
        (_animator.GetCurrentAnimatorStateInfo(1).IsName(aniName) == false)
        );

        if (_player.PlayerWallGrab.WallGrabed == false)
            FallOrIdleAnimation(_isGrounded);
        OnAttackEnded?.Invoke();
    }

    private void Update()
    {
        _animator.SetFloat("VelocityY", _rigid.velocity.y);
    }
}
