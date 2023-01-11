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
    private float _weaponSwitchingDelay = 0.2f;
    [SerializeField]
    private int _meleeAttackPower = 1;
    [SerializeField]
    private int _rangeAttackPower = 1;
    [SerializeField]
    private float _meleeAttackDelay = 0.1f;
    [SerializeField]
    private float _rangeAttackDelay = 0.1f;
    [SerializeField]
    private float _bulletSpeed = 3f;
    [SerializeField]
    private GameObject _bulletPrefab = null;

    [SerializeField]
    private UnityEvent<int> OnMeleeAttack = null;
    [SerializeField]
    private UnityEvent OnRangeAttack = null;

    private int _attackIndex = 0;
    private readonly int _maxAttackIndex = 3;

    private AttackState _attackState = AttackState.Melee;

    public void Attack()
    {
        if (_attackState == AttackState.Melee)
            MeleeAttack();
        else
            RangeAttack();
    }

    private void MeleeAttack()
    {
        // 타격 처리
        OnMeleeAttack?.Invoke(_attackIndex);
        _attackIndex = (_attackIndex + 1) % _maxAttackIndex;
    }

    private void RangeAttack()
    {
        Vector3 distance = Cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(distance.y, distance.x);
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        GameObject bullet = Instantiate(_bulletPrefab, transform.position, rot);
        bullet.AddComponent<Rigidbody>().velocity = bullet.transform.right * _bulletSpeed;
        OnRangeAttack?.Invoke();
    }

    public void WeaponSwitching()
    {
        _attackState = _attackState == AttackState.Melee ? AttackState.Range : AttackState.Melee;
        _attackIndex = 0;
        StartCoroutine(SwitchingCoroutine());
    }

    private IEnumerator SwitchingCoroutine()
    {
        _switchingable = false; 
        yield return new WaitForSeconds(_weaponSwitchingDelay);
        _switchingable = true;
    }
}

public enum AttackState
{
    Melee,
    Range
}
