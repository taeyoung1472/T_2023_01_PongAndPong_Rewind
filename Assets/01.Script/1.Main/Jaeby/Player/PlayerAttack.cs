using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static Define;

public class PlayerAttack : PlayerAction
{
    private bool _switchingable = true;
    public bool Switchingable { get => _switchingable; set => _switchingable = value; }

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

    public void Attack()
    {
        if (_locked || _delayLock || _player.PlayerActionCheck(PlayerActionType.Dash, PlayerActionType.Jump, PlayerActionType.ObjectPush, PlayerActionType.WallGrab))
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
    }

    private void RangeAttack()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 8;
        Vector3 distance = (Cam.ScreenToWorldPoint(pos) - transform.position);
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        GameObject bullet = Instantiate(_player.playerAttackSO.bulletPrefab);
        bullet.GetComponent<Bullet>().Init(transform.position, rot, _player.playerAttackSO.bulletSpeed, _player.playerAttackSO.rangeAttackPower);
        OnRangeAttack?.Invoke();
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
        _player.PlayerActionExit(PlayerActionType.Move, PlayerActionType.Jump, PlayerActionType.Dash);
        _player.PlayerActionLock(true, PlayerActionType.Move, PlayerActionType.Jump, PlayerActionType.Dash);
    }

    public void AttackEnd()
    {
        _excuting = false;
        _player.PlayerActionLock(false, PlayerActionType.Move, PlayerActionType.Jump, PlayerActionType.Dash);
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