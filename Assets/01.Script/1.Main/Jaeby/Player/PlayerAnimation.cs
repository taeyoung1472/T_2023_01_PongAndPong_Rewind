using System.Collections;
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

    private bool _moveFlipLock = false;
    public bool MoveFlipLock { get => _moveFlipLock; set => _moveFlipLock = value; }

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _animator = GetComponent<Animator>();
    }

    public void MoveAnimation(Vector2 input)
    {
        _animator.SetBool("Move", Mathf.Abs(input.x) > 0f);
        if(_moveFlipLock == false)
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

    public void DieAnimation()
    {
        Rebind();
        _animator.Play("PlayerDie");
        _animator.Update(0);
    }

    public void JumpAnimation()
    {
        Rebind();
        _animator.SetTrigger("Jump");
        _animator.Update(0);
    }

    public void ObjectPushAnimation(bool value)
    {
        _animator.SetBool("ObjectPush", value);
        _animator.Update(0);
    }

    public void WallClimbAnimation()
    {
        Rebind();
        _animator.Play("WallClimb");
        _animator.Update(0);
    }

    public void WallClimbAnimate()
    {
        _player.GetPlayerAction<PlayerWallGrab>(PlayerActionType.WallGrab).ClimbAnimate();
    }

    public void WallClimbEnd()
    {
        _player.GetPlayerAction<PlayerWallGrab>(PlayerActionType.WallGrab).WallClimbEnd();
    }

    public void SetGroundDistance(float value)
    {
        _animator.SetFloat("GroundDistance", value);
    }

    public void WallGrabAnimation(bool val)
    {
        if (val == false)
            return;
        /*if (_player.PlayerActionCheck(PlayerActionType.Attack))
        {
            _player.PlayerActionExit(PlayerActionType.Attack);
            _animator.SetTrigger("AttackForceExit");
        }*/
        _animator.SetTrigger("WallGrab");
        _animator.Update(0);
    }

    public void IdleAnimation()
    {
        _animator.Play("PlayerIdle");
        _animator.Update(0);
    }

    public void Rebind()
    {
        bool push = _animator.GetBool("ObjectPush");
        if (gameObject.activeSelf)
            _animator.Rebind();
        ObjectPushAnimation(push);
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
            _animator.CrossFade("PlayerFall", 0.2f);
            _animator.Update(0);
        }
    }

    public void AnimationGroundCheck(bool value)
    {
        _animator.SetBool("IsGround", value);
    }

    public void MeleeAttackAnimation(int index)
    {
        string name = $"MeleeAttack{index}";
        //isground 
        if ((_player.PlayerActionCheck(PlayerActionType.Move) && _player.IsGrounded) == false)
            name += "Stand";
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

    public void DieStartAnimation()
    {
        _player.playerHP.DieStart();
    }

    public void DieEndAnimation()
    {
        _player.playerHP.Restart();
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
        _animator.SetFloat("VelocityY", _player.Rigid.velocity.y);
    }

    #region Event
    float landedTime = 0.0f;
    public void OnLanded()
    {
        if (Time.time < landedTime + 0.2f)
            return;

        landedTime = Time.time;
        _player.playerAudio.OnGroundedAudio();
    }

    float jumpTime = 0.0f;
    public void OnJump()
    {
        if (Time.time < jumpTime + 0.2f)
            return;

        jumpTime = Time.time;
        _player.playerAudio.JumpAudio();
    }

    float dashAirTime = 0.0f;
    public void OnDashAir()
    {
        if (Time.time < dashAirTime + 0.2f)
            return;

        dashAirTime = Time.time;
        _player.playerAudio.DashAirAudio();
    }

    float dashGroundTime = 0.0f;
    public void OnDashGround()
    {
        if (Time.time < dashGroundTime + 0.2f)
            return;

        dashGroundTime = Time.time;
        _player.playerAudio.DashGroundAudio();
    }
    #endregion
}
