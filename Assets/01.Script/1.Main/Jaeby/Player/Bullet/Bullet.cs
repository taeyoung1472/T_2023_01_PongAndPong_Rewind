using UnityEngine;

public class Bullet : PoolAbleObject
{
    private int _dmg = 0;
    private float _speed = 0f;
    private Rigidbody _rigid = null;
    [SerializeField]
    private LayerMask _destroyMask = 0;

    public void Init(Vector3 pos, Quaternion rot, float speed, int dmg)
    {
        Transform effectTrm = PoolManager.Pop(PoolType.BulletEffect).transform;
        transform.SetPositionAndRotation(pos, rot);
        effectTrm.SetPositionAndRotation(pos, rot);
        _speed = speed;
        _dmg = dmg;
        if (_rigid == null)
            _rigid = GetComponent<Rigidbody>();
        _rigid.velocity = transform.right * _speed;
        AttackCollider.Create(_destroyMask, ColliderOwnerType.Player, transform, transform.position, transform.localScale.x, null, true, Callback);
    }

    public override void Init_Pop()
    {
    }

    public override void Init_Push()
    {
    }

    private void Callback(Collider col)
    {
        Debug.LogWarning(col);
        PoolManager.Push(poolType, gameObject);
    }
}
