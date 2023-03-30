using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator = null;

    private Coroutine _attackAniCo = null;

    [SerializeField]
    private UnityEvent OnAttackStarted = null;
    [SerializeField]
    private UnityEvent OnAttackEnded = null;

    private Player _player = null;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _animator = GetComponent<Animator>();
    }

    public void MoveAnimation(Vector2 input)
    {
        _animator.SetBool("Move", Mathf.Abs(input.x) > 0f);
        _player.PlayerRenderer.Flip(input);
    }

    public void SlideAnimation()
    {
        _animator.SetTrigger("Slide");
        _animator.Update(0);
    }

    public void DashAnimation(Vector2 input)
    {
        _animator.SetTrigger("Dash");
        _animator.Update(0);
    }

    public void JumpAnimation()
    {
        _animator.Rebind();
        _animator.Play("PlayerJump");
        _animator.Update(0);
    }

    public void ObjectPushAnimation(bool value)
    {
        _animator.SetBool("ObjectPush", value);
        _animator.Update(0);
    }

    public void WallGrabAnimation(bool val)
    {
        if (val == false)
            return;
        if (_player.PlayeActionCheck(PlayerActionType.Attack))
        {
            _player.PlayerActionExit(PlayerActionType.Attack);
            _animator.SetTrigger("AttackForceExit");
        }
        _animator.SetTrigger("WallGrab");
        _animator.Update(0);
    }

    public void IdleAnimation()
    {
        _animator.Play("PlayerIdle");
        _animator.Update(0);
    }

    public void FallOrIdleAnimation(bool isGround)
    {
        if (isGround)
        {
            _animator.SetTrigger("GoIdle");
            _animator.Update(0);
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
        if (_attackAniCo != null)
            StopCoroutine(_attackAniCo);
        _attackAniCo = StartCoroutine(AttackAnimationEndWaitCoroutine(name, AttackState.Melee));
    }

    public void RangeAttackAnimation()
    {
        _animator.Play("PlayerRangeAttack");
        _animator.Update(0);
        if (_attackAniCo != null)
            StopCoroutine(_attackAniCo);
        _attackAniCo = StartCoroutine(AttackAnimationEndWaitCoroutine("PlayerRangeAttack", AttackState.Range));
    }

    private IEnumerator AttackAnimationEndWaitCoroutine(string aniName, AttackState attackState)
    {
        OnAttackStarted?.Invoke();
        //yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(1).IsName(aniName) == false);
        yield return new WaitUntil(() =>
        (
        _animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= (attackState == AttackState.Melee ? _player.playerAttackSO.meleeAttackDelay : _player.playerAttackSO.rangeAttackDelay)) ||
        (_animator.GetCurrentAnimatorStateInfo(1).IsName(aniName) == false)
        );

        FallOrIdleAnimation(_player.IsGrounded);
        OnAttackEnded?.Invoke();
    }

    private void Update()
    {
        _animator.SetFloat("VelocityY", _player.characterController.velocity.y);
    }
}
