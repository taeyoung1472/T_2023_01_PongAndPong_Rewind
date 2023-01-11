using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator = null;
    private Rigidbody _rigid = null;

    private void Start()
    {
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

    public void WallGrabAnimation()
    {
        _animator.Play("WallAnimation");
        _animator.Update(0);
    }

    public void IdleAnimation()
    {
        _animator.Play("PlayerIdle");
        _animator.Update(0);
    }

    public void FallOrIdleAnimation(bool isGround)
    {
        if (isGround == false)
        {
            _animator.Play("PlayerFall");
            _animator.Update(0);
        }
        else
        {
            IdleAnimation();
        }
    }

    public void AnimationGroundCheck(bool value)
    {
        _animator.SetBool("IsGround", value);
    }

    private void Update()
    {
        _animator.SetFloat("VelocityY", _rigid.velocity.y);
    }
}
