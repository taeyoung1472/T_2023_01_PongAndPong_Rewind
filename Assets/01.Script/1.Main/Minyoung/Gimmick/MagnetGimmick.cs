using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetGimmick : MonoBehaviour
{
    public float speed = 5f;

    private Vector3 lookMagnetVec = Vector3.zero;

    private float radius;

    private void Awake()
    {
        radius = GetComponent<SphereCollider>().radius;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<MagnetGimmickObject>(out MagnetGimmickObject magent))
        {
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<MagnetGimmickObject>(out MagnetGimmickObject magent))
        {
            Debug.Log(magent);
            lookMagnetVec = transform.position - other.transform.position;
            float ratio = (1 - (lookMagnetVec.magnitude / radius)) * speed;
            //Debug.Log(ratio);
            Rigidbody rid = magent.GetComponent<Rigidbody>();
            rid.velocity = lookMagnetVec * ratio;
        }   
    }
}
