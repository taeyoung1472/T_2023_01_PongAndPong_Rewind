using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetGimmick : GimmickObject
{
    public float speed = 5f;

    private Vector3 lookMagnetVec = Vector3.zero;

    private float radius;
    public bool isStart = false;

    public override void Init()
    {
        radius = GetComponent<SphereCollider>().radius;
    }

    public override void Awake()
    {
        base.Awake();
        Init();
    }
    public override void InitOnPlay()
    {
        base.InitOnPlay();
        isStart = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<MagnetGimmickObject>(out MagnetGimmickObject magent))
        {
            Debug.Log(other.gameObject);
            Rigidbody rid = magent.GetComponent<Rigidbody>();
            rid.velocity = Vector3.zero;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (isRewind || !isStart)
        {
            return; 
        }
        if (other.gameObject.TryGetComponent<MagnetGimmickObject>(out MagnetGimmickObject magent))
        {
            Debug.Log(magent);
            lookMagnetVec = transform.position - other.transform.position;
            float ratio = (1 - (lookMagnetVec.magnitude / radius)) * speed;
            Rigidbody rid = magent.GetComponent<Rigidbody>();
            rid.velocity = lookMagnetVec * ratio;
        }   
    }
}
