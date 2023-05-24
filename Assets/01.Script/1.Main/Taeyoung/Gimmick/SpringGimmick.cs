using UnityEngine;

public class SpringGimmick : GimmickObject
{
    private Animator _animator;
    private LayerMask originLayer;
    private LayerMask rewindLayer;
    private void Start()
    {
        _animator = GetComponent<Animator>();

        originLayer = LayerMask.NameToLayer("Default");
        rewindLayer = LayerMask.NameToLayer("Ground");
    }
    public override void Init()
    {
        this.gameObject.layer = originLayer;
    }
    public override void InitOnRewind()
    {
        base.InitOnRewind();
        this.gameObject.layer = rewindLayer;
    }
    public override void InitOnPlay()
    {
        base.InitOnPlay();
        this.gameObject.layer = originLayer;
    }
    public override void InitOnRestart()
    {
        base.InitOnRestart();
        this.gameObject.layer = originLayer;
    }

    public void OnCollisionEnter(Collision collision)
    {
        ColliderEnter(collision.collider);
    }

    private void ColliderEnter(Collider target)
    {
        if (isRewind)
        {
            return;
        }

        if (target.transform.TryGetComponent<RigidbodyGimmickObject>(out RigidbodyGimmickObject obj))
        {
            ColEffect();
            float recordPosY = obj.RecordPosY - transform.position.y;
            recordPosY = Mathf.Clamp(recordPosY, 0, 17.5f);
            obj.Init();
            obj.AddForce(Vector3.up, recordPosY, ForceMode.VelocityChange);
        }
    }
    private void ColEffect()
    {
        _animator.Play("Jump");
    }
}
