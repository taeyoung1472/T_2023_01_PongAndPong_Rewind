using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyGimmickObject : GimmickObject
{
    Rigidbody rb;

    public override void Init()
    {
        rb.velocity = Vector3.zero;
    }
    public override void AddForce(Vector3 dir, float force, ForceMode forceMode = ForceMode.Impulse)
    {
        rb.AddForce(dir.normalized * force, forceMode);
    }
    public override void RecordTopPosition()
    {
        if (rb.velocity.y >= -1f)
        {
            recordPosY = transform.position.y;
        }
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        RecordTopPosition();
    }
}
