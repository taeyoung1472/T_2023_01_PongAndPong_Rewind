using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornGimmick : GimmickObject
{
    private Collider _col;

    public bool isCheck = false;

    [SerializeField] private float rayDistance = 0.5f;

    RaycastHit hit;

    public bool isDie;
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
                Player player = hit.collider.GetComponentInParent<Player>();
                if (isDie)
                    return;
                player.playerHP.Die();
                isDie = true;
            }
        }
    }
}
