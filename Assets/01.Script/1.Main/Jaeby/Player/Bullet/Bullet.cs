using UnityEngine;

public class Bullet : PoolAbleObject
{
    private int _dmg = 0;
    private float _speed = 0f;
    private Rigidbody _rigid = null;

    public void Init(Vector3 pos, Quaternion rot, float speed, int dmg)
    {
        transform.SetPositionAndRotation(pos, rot);
        _speed = speed;
        _dmg = dmg;
        if (_rigid == null)
            _rigid = GetComponent<Rigidbody>();
        _rigid.velocity = transform.right * _speed;
    }

    public override void Init_Pop()
    {
    }

    public override void Init_Push()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        // АјАн
        if (other.CompareTag("Player"))
            return;
        Destroy(gameObject);
    }
}
