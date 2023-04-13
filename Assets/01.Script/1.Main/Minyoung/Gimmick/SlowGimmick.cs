using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowGimmick : GimmickObject
{
    private Collider _col = null;

    [SerializeField] private float rayDistance = 1f;

    [SerializeField] private float slowSpeed = 0.5f;

    public bool isCheck = false;
    Player player =  null;

    RaycastHit hit;
    public override void Awake()
    {
        base.Awake();
        Init();
    }
    public override void Init()
    {
        _col = GetComponent<Collider>();
    }
    private void FixedUpdate()
    {
        if (isRewind)
        {
            return;
        }
        CheckObj();
    }
    public void CheckObj()
    {
        Vector3 boxcenter = _col.bounds.center;
        Vector3 halfextents = _col.bounds.extents;

        isCheck = Physics.BoxCast(boxcenter, halfextents, transform.up, out hit, transform.rotation, rayDistance);
        if (isCheck)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                player = hit.transform.GetComponentInParent<Player>();
                player.playerBuff.AddBuff(PlayerBuffType.Slow);
            }
        }
        else
        {
            
        } 
    }

    private void OnCollisionExit(Collision collision)
    {
        if (player != null)
        {
            player.playerBuff.DeleteBuff(PlayerBuffType.Slow);
            player = null;
        }
    }
}
