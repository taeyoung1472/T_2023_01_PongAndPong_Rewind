using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerJump : PlayerAction, IPlayerResetable
{
    private int _curJumpCount = 0; // 현재 점프 횟수
    public int CurJumpCount { get => _curJumpCount; set => _curJumpCount = value; }

    private bool _jumpEndCheck = false; // 점프를 한 뒤 시간이 지난 것과 바닥에 닿았을 때 둘 중 체크되면 하나가 안 되게

    [SerializeField]
    private Transform _jumpEffectPos = null;
    [SerializeField]
    private UnityEvent OnJump = null;
    [SerializeField]
    private UnityEvent OnWallGrabJump = null;
    private Coroutine _jumpCoroutine = null;
    private Coroutine _moveLockCoroutine = null;

    [SerializeField]
    private float _upLayLength = 0.1f;
    [SerializeField]
    private LayerMask _upLayerMask = 0;

    private float _jumpInputTime = 0f;
    private bool _jumpKeyUped = false;

    private bool _firstJump = true;

    private void Update()
    {
        if (_excuting)
            _jumpInputTime += Time.deltaTime;
        if (_jumpKeyUped && _jumpInputTime >= _player.playerMovementSO.jumpHoldTime * 0.5f)
        {
            _jumpKeyUped = false;
            _jumpInputTime = 0f;
            JumpEnd();
        }
    }

    private void FixedUpdate()
    {
        HeadTouchCheck();
    }

    private void HeadTouchCheck()
    {
        if (_excuting == false)
            return;

        bool result = Physics.Raycast(_player.transform.position,
            _player.transform.up, _upLayLength + _player.Col.height, _upLayerMask);
        if (result)
        {
            Debug.Log("머리 꽁");
            ActionExit();
            _player.PlayerAnimation.Rebind();
            _player.PlayerAnimation.FallOrIdleAnimation(_player.IsGrounded);
        }
    }

    public void JumpKeyUp()
    {
        _jumpKeyUped = true;
    }

    public void OnGrounded(bool val)
    {
        if (val == false)
            return;
        _firstJump = true;
        _jumpKeyUped = false;
        _jumpInputTime = 0f;
        _curJumpCount = 0;
        _player.GravityModule.GravityScale = _player.GravityModule.OriginGravityScale;
    }
    public void ForceJump(Vector2 dir, float jumpPower, float jumpHoldTime)
    {
        _jumpEndCheck = false;
        _excuting = true;
        if (_jumpCoroutine != null)
            StopCoroutine(_jumpCoroutine);
        if (_moveLockCoroutine != null)
        {
            StopCoroutine(_moveLockCoroutine);
            _player.PlayerActionLock(false, PlayerActionType.Move);
        }
        _player.VeloCityResetImm(y: true);

        _jumpCoroutine = StartCoroutine(JumpCoroutine(dir, jumpPower, jumpHoldTime));
        OnJump?.Invoke();
    }

    public void JumpStart()
    {
        if (_locked)
            return;
        if (_firstJump)
        {
            if (_player.IsGrounded == false)
                _curJumpCount++;
            _firstJump = false;
        }
        if (_curJumpCount >= _player.playerMovementSO.jumpCount)
            return;

        _jumpEndCheck = false;
        _excuting = true;
        _curJumpCount++;
        _jumpKeyUped = false;
        _jumpInputTime = 0f;

        if (_jumpCoroutine != null)
            StopCoroutine(_jumpCoroutine);
        if (_moveLockCoroutine != null)
        {
            StopCoroutine(_moveLockCoroutine);
            _player.PlayerActionLock(false, PlayerActionType.Move);
        }

        OnJump?.Invoke();
        if (_player.PlayerActionCheck(PlayerActionType.WallGrab)) // 월점프!!
        {
            _player.GetPlayerAction<PlayerWallGrab>().WallExit();
            _player.VeloCityResetImm(x: true, y: true);
            _player.PlayerRenderer.ForceFlip();
            Vector2 jumpDir = _player.playerMovementSO.wallJumpPower;
            jumpDir.x *= _player.PlayerRenderer.Forward.x;
            _jumpCoroutine = StartCoroutine(JumpCoroutine(jumpDir, _player.playerMovementSO.wallGrabJumpPower, _player.playerMovementSO.jumpHoldTime));
            _moveLockCoroutine = StartCoroutine(MoveLockCoroutine());
            OnWallGrabJump?.Invoke();
        }
        else
        {
            _player.VeloCityResetImm(y: true);
            _jumpCoroutine = StartCoroutine(JumpCoroutine(_player.transform.up, _player.playerMovementSO.jumpPower, _player.playerMovementSO.jumpHoldTime));
        }
    }

    private IEnumerator MoveLockCoroutine()
    {
        _player.PlayerActionLock(true, PlayerActionType.Move);
        yield return new WaitForSeconds(_player.playerMovementSO.moveLockTime);
        _player.PlayerActionLock(false, PlayerActionType.Move);
    }

    private IEnumerator JumpCoroutine(Vector2 dir, float jumpPower, float jumpHoldTime)
    {
        float time = 1f;
        while (time > 0f)
        {
            Vector2 final = new Vector2(Mathf.Sqrt(1 - (float)Math.Pow(time - 1, 2)) * dir.x * jumpPower, Mathf.Sqrt(1 - (float)Math.Pow(time - 1, 2)) * dir.y * jumpPower);
            _player.VelocitySetExtra(final.x, final.y);
            time -= Time.deltaTime * (1f / jumpHoldTime);
            yield return null;
        }
        JumpEnd();
    }

    public void JumpEnd()
    {
        if (_jumpEndCheck)
            return;
        _jumpEndCheck = true;
        _excuting = false;
        if (_jumpCoroutine != null)
            StopCoroutine(_jumpCoroutine);
        if (_moveLockCoroutine != null)
        {
            StopCoroutine(_moveLockCoroutine);
            _player.PlayerActionLock(false, PlayerActionType.Move);
        }
        _player.VelocitySetExtra(0f, 0f);
        if (_player.IsGrounded)
            OnGrounded(true);
    }

    public void JumpCountSetting(int cnt)
    {
        _firstJump = false;
        _curJumpCount = cnt;
        if (_curJumpCount < 0)
            _curJumpCount = 0;
    }

    public void MoreJump(int cnt)
    {
        _firstJump = false;
        _curJumpCount -= cnt;
        if (_curJumpCount < 0)
            _curJumpCount = 0;
    }

    public void TryGravityUp(Vector2 input)
    {
        if (_player.IsGrounded || input.y > -0.1f || _player.PlayerActionCheck(PlayerActionType.WallGrab))
            return;
        _player.GravityModule.GravityScale = _player.playerMovementSO.downGravityScale;
    }

    public override void ActionExit()
    {
        _jumpEndCheck = false;
        _jumpInputTime = 0f;
        _jumpKeyUped = false;
        JumpEnd();
    }

    public void SpawnJumpEffect()
    {
        //flip이면 x -90

        Vector3 pos = _jumpEffectPos.position;
        Quaternion rot = Quaternion.identity;
        Transform trm = PoolManager.Pop(PoolType.JumpEffect).transform;
        if (_player.PlayerActionCheck(PlayerActionType.WallGrab))
        {
            rot = _player.PlayerRenderer.GetFlipedRotation(DirType.Forward, RotAxis.Z);
            pos += transform.up * 0.5f;
        }
        if (_player.PlayerRenderer.flipDirection == DirectionType.Up)
        {
            rot = Quaternion.Euler(180f, 0f, 0f);
        }
        trm.SetPositionAndRotation(pos, rot);
    }

    public void EnableReset()
    {
        _curJumpCount = 0;
    }

    public void DisableReset()
    {
    }
}
