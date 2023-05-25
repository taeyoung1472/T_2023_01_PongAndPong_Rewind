using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePlateGimmick : GimmickObject
{
    [SerializeField] private float destroyTime = 0f;
    [SerializeField] private float basicTime;

    private Collider _col;

    public bool isCheck = false;

    [SerializeField] private float rayDistance = 0.5f;

    RaycastHit hit;

   public bool isEnter = false;

    public override void Awake()
    {
        base.Awake();
        Init();
    }
    public override void Init()
    {
        _col = GetComponent<BoxCollider>();
    }
    public override void InitOnPlay()
    {
        destroyTime = basicTime;
        isEnter = false;
        base.InitOnPlay();
    }
    public override void InitOnRewind()
    {
        base.InitOnRewind();
    }

    public void Update()
    {
        if (isRewind)
        {
            return;
        }

        if (isEnter)
        {
            destroyTime -= Time.deltaTime;
            if(destroyTime <= 0.0f)
            {
                gameObject.SetActive(false);
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
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
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

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("발판에서 내려감");
            isEnter = false;
            destroyTime = basicTime;
        }
    }
}
