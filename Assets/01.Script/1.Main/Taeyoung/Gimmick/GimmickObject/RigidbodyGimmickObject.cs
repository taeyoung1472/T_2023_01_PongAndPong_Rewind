using UnityEngine;

public class RigidbodyGimmickObject : GimmickObject
{
    protected float recordPosY;
    public float RecordPosY { get { return recordPosY; } }

    Rigidbody rb;
    RigidbodyConstraints constraints;
    [SerializeField] private bool isRewindPlayer = false;

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

    public override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        if (rb != null)
            constraints = rb.constraints;

    }

    public void FixedUpdate()
    {
        RecordTopPosition();
    }

    public override void InitOnRewind()
    {
        if (rb != null && isRewindPlayer)
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

    public void OnCollisionEnter(Collision collision)
    {
        if (transform.position.y - recordPosY < -3f)
        {
            AudioManager.PlayAudioRandPitch(SoundType.OnObjectImpact);
            TimeStampManager.Instance.SetStamp(StampType.dropBox);
        }
    }
}
