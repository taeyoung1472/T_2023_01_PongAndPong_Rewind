using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static Define;
using static UnityEngine.Rendering.DebugUI.Table;

public class PlayerAttack : PlayerAction
{
    [SerializeField]
    private UnityEvent<int> OnMeleeAttack = null;
    [SerializeField]
    private UnityEvent OnRangeAttack = null;

    private int _attackIndex = -1; // 함수에 들어가서 +1 해주기에 -1부터 시작
    private readonly int _maxAttackIndex = 3;
    private bool _delayLock = false;

    private AttackState _attackState = AttackState.Melee;
    private Coroutine _attackDelayCo = null;

    #region 스위칭
    private bool _switchingable = true;
    public bool Switchingable { get => _switchingable; set => _switchingable = value; }
    private Coroutine _switchingCo = null;
    #endregion

    #region 원거리 공격
    [SerializeField]
    private Transform _shootingPointTrm = null;
    [SerializeField]
    private Coroutine _flipLockCo = null;
    private float _flipLockTime = 0.4f;
    private Transform _mousePositionTrm = null;
    private bool _iKFliped = false;
    private bool _shooting = false;
    #endregion

    public void Attack()
    {
        if (_locked || _delayLock || _player.PlayerActionCheck(PlayerActionType.Dash, PlayerActionType.ObjectPush, PlayerActionType.WallGrab))
            return;

        _excuting = true;
        _player.VelocitySetMove(0f, 0f);

        if (_attackState == AttackState.Melee)
            MeleeAttack();
        else
            RangeAttack();

        if (_attackDelayCo != null)
            StopCoroutine(_attackDelayCo);
        _attackDelayCo = StartCoroutine(DelayCoroutine());
    }

    private void MeleeAttack()
    {
        // 타격 처리
        _attackIndex = (_attackIndex + 1) % _maxAttackIndex;
        OnMeleeAttack?.Invoke(_attackIndex);
        player.playerAudio.AttackAudio();

        AttackCollider.Create(0, ColliderOwnerType.Player, null, _player.transform.position + _player.PlayerRenderer.Forward, 0.9f, 0.5f, false, null);
    }

    private void RangeAttack()
    {
        _shooting = true;
        StartCoroutine(RangeAttackCoroutine());
    }

    private IEnumerator RangeAttackCoroutine()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = (_player.transform.position - Cam.transform.position).z;
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);

        if (_mousePositionTrm == null)
            _mousePositionTrm = new GameObject("MousePositionTrm").transform;
        _player.PlayerRenderer.Flip(target - _player.transform.position);
        _player.PlayerAnimation.MoveFlipLock = true;
        _mousePositionTrm.position = new Vector3(target.x, target.y, _player.transform.position.z);
        _player.animationIK.LookTrm = _mousePositionTrm;
        _player.animationIK.HandL = _mousePositionTrm;
        _player.animationIK.HandR = _mousePositionTrm;
        _player.animationIK.RotationLock = true;
        _player.animationIK.SetIKWeightOne();
        _iKFliped = _player.PlayerRenderer.Fliped;

        yield return new WaitForEndOfFrame();

        Vector3 distance = target - _shootingPointTrm.position;
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        if (_flipLockCo != null)
            StopCoroutine(_flipLockCo);
        _flipLockCo = StartCoroutine(FlipLockCoroutine());
        Bullet bullet = PoolManager.Pop(PoolType.PlayerBullet).GetComponent<Bullet>();
        bullet.GetComponent<Bullet>().Init(_shootingPointTrm.position, rot, _player.playerAttackSO.bulletSpeed, _player.playerAttackSO.rangeAttackPower);
        OnRangeAttack?.Invoke();
    }

    private void Update()
    {
        if (_excuting == false || _shooting == false)
            return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = (_player.transform.position - Cam.transform.position).z;
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);
        _mousePositionTrm.position = new Vector3(target.x, target.y, _player.transform.position.z);
        _player.PlayerRenderer.Flip(target - _player.transform.position);

        /*bool fliped = _mousePositionTrm.position.x < _player.transform.position.x; // flip -> 왼쪽
        if (_iKFliped != fliped)
        {
            Debug.Log("원거리 IK 해제");
            if (_flipLockCo != null)
                StopCoroutine(_flipLockCo);
            _player.animationIK.SetIKWeightZero();
            _player.animationIK.RotationLock = false;
            _player.PlayerAnimation.MoveFlipLock = false;
            _shooting = false;
            _excuting = false;
        }*/
    }

    private IEnumerator FlipLockCoroutine()
    {
        yield return new WaitForSeconds(_flipLockTime);
        _player.animationIK.SetIKWeightZero();
        _player.animationIK.RotationLock = false;
        _player.PlayerAnimation.MoveFlipLock = false;
        _shooting = false;
        _excuting = false;
    }

    public void WeaponSwitching()
    {
        if (_switchingable == false)
            return;

        _attackState = _attackState == AttackState.Melee ? AttackState.Range : AttackState.Melee;
        _attackIndex = -1;
        if (_switchingCo != null)
            StopCoroutine(_switchingCo);
        StartCoroutine(SwitchingCoroutine());
    }

    private IEnumerator SwitchingCoroutine()
    {
        _switchingable = false;
        yield return new WaitForSeconds(_player.playerAttackSO.weaponSwitchingDelay);
        _switchingable = true;
    }

    private IEnumerator DelayCoroutine()
    {
        _delayLock = true;
        float time = 0f;
        if (_attackState == AttackState.Melee)
            time = _player.playerAttackSO.meleeAttackDelay;
        else
            time = _player.playerAttackSO.rangeAttackDelay;
        yield return new WaitForSeconds(time);
        _delayLock = false;
    }

    public void AttackStart()
    {
        _excuting = true;
    }

    public void AttackEnd()
    {
        _excuting = false;
    }

    public override void ActionExit()
    {
        if (_switchingCo != null)
            StopCoroutine(_switchingCo);
        if (_attackDelayCo != null)
            StopCoroutine(_attackDelayCo);
        _switchingable = true;
        _delayLock = false;
        _excuting = false;
        _shooting = false;
        _player.animationIK.SetIKWeightZero();
        _player.animationIK.RotationLock = false;
        _player.PlayerAnimation.MoveFlipLock = false;
    }
}