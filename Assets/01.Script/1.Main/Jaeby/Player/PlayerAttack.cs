using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Define;

public class PlayerAttack : MonoBehaviour
{
    private bool _attackable = true;
    public bool Attackable { get => _attackable; set => _attackable = value; }
    private bool _switchingable = true;
    public bool Switchingable { get => _switchingable; set => _switchingable = value; }

    [SerializeField]
    private PlayerAttackSO _playerAttackSO = null;

    [SerializeField]
    private UnityEvent<int> OnMeleeAttack = null;
    [SerializeField]
    private UnityEvent OnRangeAttack = null;

    private int _attackIndex = 0;
    private readonly int _maxAttackIndex = 3;

    private AttackState _attackState = AttackState.Melee;
    private Coroutine _switchingCo = null;
    private Coroutine _attackDelayCo = null;
    private Player _player = null;

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    public void Attack()
    {
        if (_attackable == false)
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
        OnMeleeAttack?.Invoke(_attackIndex);
        _attackIndex = (_attackIndex + 1) % _maxAttackIndex;
    }

    private void RangeAttack()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 8;
        Vector3 distance = (Cam.ScreenToWorldPoint(pos) - transform.position);
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        GameObject bullet = Instantiate(_playerAttackSO.bulletPrefab);
        bullet.GetComponent<Bullet>().Init(transform.position, rot, _playerAttackSO.bulletSpeed, _playerAttackSO.rangeAttackPower);
        OnRangeAttack?.Invoke();
    }

    public void WeaponSwitching()
    {
        if (_switchingable == false)
            return;

        _attackState = _attackState == AttackState.Melee ? AttackState.Range : AttackState.Melee;
        _attackIndex = 0;
        if (_switchingCo != null)
            StopCoroutine(_switchingCo);
        StartCoroutine(SwitchingCoroutine());
    }

    private IEnumerator SwitchingCoroutine()
    {
        _switchingable = false; 
        yield return new WaitForSeconds(_playerAttackSO.weaponSwitchingDelay);
        _switchingable = true;
    }

    private IEnumerator DelayCoroutine()
    {
        _attackable = false;
        _player.PlayerMove.Moveable = false;
        float time = 0f;
        if (_attackState == AttackState.Melee)
            time = _playerAttackSO.meleeAttackDelay;
        else
            time = _playerAttackSO.rangeAttackDelay;
        yield return new WaitForSeconds(time);
        _attackable = !_player.PlayerWallGrab.WallGrabed;
        _player.PlayerMove.Moveable = !_player.PlayerWallGrab.WallGrabed;
    }
}

public enum AttackState
{
    Melee,
    Range
}
