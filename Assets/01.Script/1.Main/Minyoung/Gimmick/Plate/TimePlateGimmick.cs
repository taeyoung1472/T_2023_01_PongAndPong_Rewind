using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePlateGimmick : GimmickObject
{
    [SerializeField] private float destroyTime = 0f;
    private float basicTime;

    private Collider _col;

    public bool isCheck = false;

    [SerializeField] private float rayDistance = 0.5f;

    RaycastHit hit;

    bool isEnter = false;

    private void Awake()
    {
        Init();
    }
    public override void Init()
    {
        _col = GetComponent<BoxCollider>();
        basicTime = destroyTime;
    }

    public void Update()
    {
        if (isEnter)
        {
            destroyTime -= Time.deltaTime;
            if(destroyTime <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }
    
    private void FixedUpdate()
    {
        Vector3 boxcenter = _col.bounds.center;
        Vector3 halfextents = _col.bounds.extents;

        isCheck = Physics.BoxCast(boxcenter, halfextents, transform.up, out hit, transform.rotation, rayDistance);

        if (isCheck)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("GimmickPlayer"))
            {
                Debug.Log("발판 충돌");
                isEnter = true;
            }
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
 
 

    private void OnTriggerExit(Collider other)
    {
        isEnter = false;
        destroyTime = basicTime;
    }
}
