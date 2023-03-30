using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGimmickObject : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 gravityVec = Vector3.zero;
    public Vector3 GravityVec { get => gravityVec; set => gravityVec = value; }
    [SerializeField] private float gravityScale;
    public float GravityScale { get => gravityScale; set => gravityScale = value; }
    [SerializeField] private float orignGravityScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gravityScale = orignGravityScale;
    }
    private void Start()
    {
        gravityVec = new Vector3(0, -9.8f, 0f);
    }
    private void FixedUpdate()
    {
        rb.velocity = gravityVec;// * gravityScale;
    }
}
