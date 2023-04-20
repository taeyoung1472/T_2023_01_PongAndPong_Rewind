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
        transform.SetPositionAndRotation(pos, rot);
        _speed = speed;
        _dmg = dmg;
        if (_rigid == null)
            _rigid = GetComponent<Rigidbody>();
        _rigid.velocity = transform.right * _speed;
        Debug.Log(_rigid.velocity);
    }

    public override void Init_Pop()
    {
    }

    public override void Init_Push()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer | _destroyMask) == 0)
            return;

        PoolManager.Push(poolType, gameObject);
    }
}
