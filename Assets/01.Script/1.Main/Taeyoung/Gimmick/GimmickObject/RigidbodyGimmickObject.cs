using UnityEngine;

public class RigidbodyGimmickObject : GimmickObject
{
    protected float recordPosY;
    public float RecordPosY { get { return recordPosY; } }

    Rigidbody rb;

    public override void Init()
    {
        rb.velocity = Vector3.zero;
    }

    public virtual void AddForce(Vector3 dir, float force, ForceMode forceMode = ForceMode.Impulse)
    {
        rb.AddForce(dir.normalized * force, forceMode);
    }

    public virtual void RecordTopPosition()
    {
        if (rb.velocity.y >= -1f)
        {
            recordPosY = transform.position.y;
        }
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        RewindManager.Instance.InitRewind += InitOnRewind;
    }

    public void FixedUpdate()
    {
        RecordTopPosition();
    }

    public override void InitOnRewind()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
