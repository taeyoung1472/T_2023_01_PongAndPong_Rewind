using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static Define;
using static UnityEngine.Rendering.DebugUI.Table;

public class PlayerAttack : PlayerAction, IPlayerResetable
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
    private GameObject _pistolObj = null;
    [SerializeField]
    private Transform _shootingPointTrm = null;
    private Coroutine _flipLockCo = null;
    private Coroutine _iKEndAnimationCoroutine = null;
    [SerializeField]
    private float _flipLockTime = 0.4f;
    [SerializeField]
    private float _ikEndAnimationTime = 0.3f;
    private Transform _mousePositionTrm = null;
    private bool _shooting = false;
    [SerializeField]
    private float _ikCancelMaxAngle = 30f;
    private bool _observerStarting = false;
    #endregion

    private void Start()
    {

        _pistolObj.SetActive(false);
    }

    public void Attack()
    {
        if (_locked || _delayLock || _player.PlayerActionCheck(PlayerActionType.ObjectPush, PlayerActionType.WallGrab))
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
        _observerStarting = false;
        if (_iKEndAnimationCoroutine != null)
            StopCoroutine(_iKEndAnimationCoroutine);
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

        yield return new WaitForEndOfFrame();

        Vector3 distance = target - _shootingPointTrm.position;
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        if (AngleCheck())
        {
            EndIK();
        }
        else
        {
            if (_flipLockCo != null)
                StopCoroutine(_flipLockCo);
            _flipLockCo = StartCoroutine(FlipLockCoroutine());
            _observerStarting = true;
        }

        Bullet bullet = PoolManager.Pop(PoolType.PlayerBullet).GetComponent<Bullet>();
        bullet.GetComponent<Bullet>().Init(_shootingPointTrm.position, rot, _player.playerAttackSO.bulletSpeed, _player.playerAttackSO.rangeAttackPower);
        OnRangeAttack?.Invoke();
    }

    private void LateUpdate()
    {
        if (_excuting == false || _shooting == false || _observerStarting == false)
            return;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = (_player.transform.position - Cam.transform.position).z;
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);
        _mousePositionTrm.position = new Vector3(target.x, target.y, _player.transform.position.z);
        if (AngleCheck())
        {
            _observerStarting = false;
            EndIK();
        }
        _player.PlayerRenderer.Flip(target - _player.transform.position);
    }

    private bool AngleCheck()
    {
        float angle = Vector2.Angle((Vector2)(_mousePositionTrm.position - _player.transform.position), (Vector2)_player.transform.up);
        if (angle < _ikCancelMaxAngle || angle > (180 - _ikCancelMaxAngle))
        {
            return true;
        }
        return false;
    }

    private IEnumerator FlipLockCoroutine()
    {
        yield return new WaitForSeconds(_flipLockTime);
        EndIK();
    }

    private void EndIK()
    {
        if (_flipLockCo != null)
            StopCoroutine(_flipLockCo);
        if (_iKEndAnimationCoroutine != null)
            StopCoroutine(_iKEndAnimationCoroutine);
        _iKEndAnimationCoroutine = StartCoroutine(EndIkCoroutine());

        _player.PlayerAnimation.MoveFlipLock = false;
        _shooting = false;
        _excuting = false;
        _observerStarting = false;
    }

    private IEnumerator EndIkCoroutine()
    {
        Debug.Log("무슨 일임?");
        float time = 1f;
        while (time >= 0f)
        {
            _player.animationIK.SetIKWeight(time);
            time -= Time.deltaTime * (1 / _ikEndAnimationTime);
            yield return null;
        }
        _player.animationIK.SetIKWeightZero();
        _player.animationIK.RotationLock = false;
    }

    public void WeaponSwitching()
    {
        if (_switchingable == false)
            return;

        _attackState = _attackState == AttackState.Melee ? AttackState.Range : AttackState.Melee;
        if (_attackState == AttackState.Range)
        {
            _pistolObj.SetActive(true);
        }
        else if (_attackState == AttackState.Melee)
        {
            _pistolObj.SetActive(false);
        }
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
        if (_iKEndAnimationCoroutine != null)
            StopCoroutine(_iKEndAnimationCoroutine);
        if (_flipLockCo != null)
            StopCoroutine(_flipLockCo);
        _switchingable = true;
        _delayLock = false;
        _excuting = false;
        _shooting = false;
        _observerStarting = false;
        _player.animationIK.SetIKWeightZero();
        _player.animationIK.RotationLock = false;
        _player.PlayerAnimation.MoveFlipLock = false;
    }

    public void EnableReset()
    {
        _pistolObj.SetActive(false);
        _attackState = AttackState.Range;
        WeaponSwitching();
    }

    public void DisableReset()
    {

    }
}