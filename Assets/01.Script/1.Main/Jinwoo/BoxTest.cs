using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTest : MonoBehaviour
{
    public Rigidbody rig;
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            rig.AddExplosionForce(1000, transform.position, 10);
        }
    }
}
