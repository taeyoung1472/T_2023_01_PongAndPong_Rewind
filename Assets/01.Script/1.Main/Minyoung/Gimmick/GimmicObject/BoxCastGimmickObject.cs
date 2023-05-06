using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCastGimmickObject : GimmickObject
{
    private Collider _col;
    [SerializeField] private float rayDistance;
    public bool isCol = false; 
    public override void Awake()
    {
        base.Awake();
        Init();
    }

    public override void Init()
    {
        _col = GetComponent<Collider>();
    }

    void FixedUpdate()
    {
        UpBoxCast();
    }
    public void UpBoxCast()
    {
        RaycastHit hit;
        Vector3 boxCenter = _col.bounds.center;
        Vector3 halfExtents = _col.bounds.extents;

        isCol = Physics.BoxCast(boxCenter, halfExtents, transform.up, out hit, transform.rotation, rayDistance);
        if (isCol)
        {
            Debug.Log("sex");
            if (hit.collider.CompareTag("GimmickPlayerCol"))
            {
                BoxHit();
            }
        }
        else
        {
            BoxExit();
        }

    }
    private void OnDrawGizmos()
    {
        
    }
    public virtual void BoxHit() { }
    public virtual void BoxExit() { }
    
}
