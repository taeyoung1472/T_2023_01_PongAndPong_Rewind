using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static Define;

public class PlayerAttack : PlayerAction
{
    private bool _switchingable = true;
    public bool Switchingable { get => _switchingable; set => _switchingable = value; }

    [SerializeField]
    private Transform _shootingPointTrm = null;

    [SerializeField]
    private UnityEvent<int> OnMeleeAttack = null;
    [SerializeField]
    private UnityEvent OnRangeAttack = null;

    private int _attackIndex = -1; // 함수에 들어가서 +1 해주기에 -1부터 시작
    private readonly int _maxAttackIndex = 3;

    private AttackState _attackState = AttackState.Melee;
    private Coroutine _switchingCo = null;
    private Coroutine _attackDelayCo = null;

    private bool _delayLock = false;

    private Transform _mousePositionTrm = null;

    public void Attack()
    {
        if (_locked || _delayLock || _player.PlayerActionCheck(PlayerActionType.Dash, PlayerActionType.ObjectPush, PlayerActionType.WallGrab))
            return;

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
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = _shootingPointTrm.position;
        Quaternion camRot = Quaternion.Euler(Cam.transform.rotation.eulerAngles.x, Cam.transform.rotation.eulerAngles.y, Cam.transform.rotation.eulerAngles.z);
        mousePos.z = (playerPos - Cam.transform.position).z;
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 distance = target - playerPos;
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        Bullet bullet = PoolManager.Pop(PoolType.PlayerBullet).GetComponent<Bullet>();
        bullet.GetComponent<Bullet>().Init(playerPos, rot, _player.playerAttackSO.bulletSpeed, _player.playerAttackSO.rangeAttackPower);
        OnRangeAttack?.Invoke();
        _player.PlayerRenderer.Flip(target - _player.transform.position);
        if (_mousePositionTrm == null)
        {
            _mousePositionTrm = new GameObject("MousePositionTrm").transform;
        }
        _mousePositionTrm.position = target;
        _player.animationIK.LookTrm = _mousePositionTrm;
        _player.animationIK.HandL = _mousePositionTrm;
        _player.animationIK.HandR = _mousePositionTrm;
        _player.animationIK.SetIKWeightOne();
        //AttackCollider.Create(0, _player.gameObject, bullet.transform, Vector3.zero, 0.5f, null, true, null);
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
        _player.VeloCityResetImm(true, true);
        //_player.PlayerActionExit(PlayerActionType.Move, PlayerActionType.Jump, PlayerActionType.Dash);
        //_player.PlayerActionLock(true, PlayerActionType.Move, PlayerActionType.Jump, PlayerActionType.Dash);
    }

    public void AttackEnd()
    {
        _excuting = false;
        //_player.PlayerActionLock(false, PlayerActionType.Move, PlayerActionType.Jump, PlayerActionType.Dash);
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
    }
}