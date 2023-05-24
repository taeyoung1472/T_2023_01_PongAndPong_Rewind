using UnityEngine;

public class Bullet : PoolAbleObject
{
    private int _dmg = 0;
    private float _speed = 0f;
    private Rigidbody _rigid = null;
    [SerializeField]
    private LayerMask _destroyMask = 0;
    private TrailRenderer _trailRenderer = null;

    public void Init(Vector3 pos, Quaternion rot, float speed, int dmg)
    {
        Transform effectTrm = PoolManager.Pop(PoolType.BulletEffect).transform;
        transform.SetPositionAndRotation(pos, rot);
        effectTrm.SetPositionAndRotation(pos + transform.forward * 0.12f, rot);
        if (_trailRenderer == null)
            _trailRenderer = GetComponent<TrailRenderer>();
        _trailRenderer.Clear();
        _speed = speed;
        _dmg = dmg;
        if (_rigid == null)
            _rigid = GetComponent<Rigidbody>();
        _rigid.velocity = transform.right * _speed;
        AttackCollider.Create(_destroyMask, ColliderType.PlayerBullet, transform, transform.position, transform.localScale.x, null, true, Callback);
    }

    public override void Init_Pop()
    {
    }

    public override void Init_Push()
    {
    }

    private void Callback(Collider col)
    {
        PoolManager.Push(poolType, gameObject);
    }
}
