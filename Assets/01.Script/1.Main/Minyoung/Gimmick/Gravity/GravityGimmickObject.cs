using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGimmickObject : GimmickObject
{
    private Rigidbody rb;
    private Vector3 gravityDir = Vector3.zero;
    public Vector3 GravityDir { get => gravityDir; set => gravityDir = value; }
    [SerializeField] private float gravityScale;
    public float GravityScale { get => gravityScale; set => gravityScale = value; }
   
    [SerializeField] private float orignGravityScale;
    [SerializeField] public float OrignGravityScale => orignGravityScale;

    private void Start()
    {
        Init();
        gravityDir = new Vector3(0, -9.8f, 0f);
    }
    private void FixedUpdate()
    {
        rb.velocity = gravityDir * gravityScale;
    }

    public override void Init()
    {
        rb = GetComponent<Rigidbody>();
        gravityScale = orignGravityScale;
    }
}
