using UnityEngine;

public class RigidbodyGimmickObject : GimmickObject
{
    protected float recordPosY;
    public float RecordPosY { get { return recordPosY; } }

    Rigidbody rb;
    RigidbodyConstraints constraints;

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
        if (rb != null)
            constraints = rb.constraints;
        if (RewindManager.Instance)
        {
            RewindManager.Instance.InitRewind += InitOnRewind;
            RewindManager.Instance.InitPlay += InitOnPlay;
        }
    }

    public void FixedUpdate()
    {
        RecordTopPosition();
    }

    public override void InitOnRewind()
    {
        if (rb != null)
            rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public override void InitOnPlay()
    {
        DisFreeze();
    }

    public void DisFreeze()
    {
        if (rb != null)
            rb.constraints = constraints;
    }
    private void OnEnable()
    {
        if (RewindManager.Instance != null)
            RewindManager.Instance.RestartPlay += DisFreeze;
    }

    private void OnDisable()
    {
        if (RewindManager.Instance != null)
            RewindManager.Instance.RestartPlay -= DisFreeze;

    }
}
