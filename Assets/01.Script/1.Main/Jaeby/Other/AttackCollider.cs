using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : PoolAbleObject
{
    private ColliderOwnerType _ownerType = ColliderOwnerType.None;
    public ColliderOwnerType OwnerType => _ownerType;

    private SphereCollider _col = null;

    private float? _lifeTime = 0f;
    private bool _isOnceTouch = false;

    private float _timer = 0f;

    private Action<Collider> _callback = null;

    private LayerMask _mask = 0;

    /// <summary>
    /// 어택 콜라이더 생성
    /// </summary>
    /// <param name="레이어 마스크"></param>
    /// <param name="생성하는 오브젝트"></param>
    /// <param name="부모 트랜스폼"></param>
    /// <param name="포지션"></param>
    /// <param name="콜라이더 반지름"></param>
    /// <param name="지속시간"></param>
    /// <param name="한 번만 터치하면 끝인가요?"></param>
    /// <param name="이벤트"></param>
    public static void Create(LayerMask mask, ColliderOwnerType ownerType, Transform parent, Vector3 pos, float radius, float? lifeTime, bool onceTouch, Action<Collider> CallbackAction)
    {
        AttackCollider collider = PoolManager.Pop(PoolType.AttackCollider).GetComponent<AttackCollider>();
        collider.InitCollider(mask, ownerType, parent, pos, radius, lifeTime, onceTouch, CallbackAction);
    }

    public void InitCollider(LayerMask mask, ColliderOwnerType ownerType, Transform parent, Vector3 pos, float radius, float? lifeTime, bool onceTouch, Action<Collider> CallbackAction)
    {
        _mask = mask;
        _ownerType = ownerType;
        transform.position = pos;
        _col.radius = radius;
        _lifeTime = lifeTime;
        transform.SetParent(parent);
        _isOnceTouch = onceTouch;
        _callback = CallbackAction;
        _timer = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer & _mask) == 0)
            return;

        _callback?.Invoke(other);
        if (_isOnceTouch)
            PoolManager.Push(poolType, gameObject);
    }

    private void Update()
    {
        if (_lifeTime == null)
            return;

        _timer += Time.deltaTime;
        if (_timer >= _lifeTime.Value)
            PoolManager.Push(poolType, gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_col == null)
            return;
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawSphere(transform.position, _col.radius);
    }
#endif

    public override void Init_Pop()
    {
        if (_col == null)
            _col = GetComponent<SphereCollider>();
        _col.enabled = true;
    }

    public override void Init_Push()
    {
        _timer = 0f;
        _callback = null;
    }
}
