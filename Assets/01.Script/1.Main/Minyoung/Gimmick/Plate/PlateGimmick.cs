using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateGimmick : GimmickObject
{
    [SerializeField] private int cnt = 0;

    private Collider _col;

    public bool isCheck = false;

    [SerializeField] private float rayDistance = 0.5f;

    public bool isCol = false;

    RaycastHit hit;

    private void Awake()
    {
        Init();
    }
    public override void Init()
    {
        _col = GetComponent<BoxCollider>();
    }

    private void FixedUpdate()
    {
        Vector3 boxCenter = _col.bounds.center;
        Vector3 halfExtents = _col.bounds.extents;

        isCheck = Physics.BoxCast(boxCenter, halfExtents, transform.up, out hit, transform.rotation, rayDistance);

        if (isCheck)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("GimmickPlayer"))
            {
                if (isCol)
                    return;
                isCol = true;
                Debug.Log("발판 충돌");
                cnt--;
                if (cnt <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            isCol = false;
        }
    }
    private void OnDrawGizmos()
    {
        if (isCheck)
        {
            Gizmos.DrawRay(transform.position, transform.up * hit.distance);
            Gizmos.DrawWireCube(transform.position + transform.up * hit.distance, transform.localScale);
        }
        else
        {
            Gizmos.DrawRay(transform.position, transform.up * rayDistance);
            Gizmos.DrawWireCube(transform.position + transform.up * rayDistance, transform.localScale);
        }
    }
}
