using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowGimmick : GimmickObject
{
    private Collider _col = null;

    [SerializeField] private float rayDistance = 1f;

    [SerializeField] private float slowSpeed = 0.5f;

    public bool isCheck = false;

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
      //  CheckObj();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("충돌함");
            player = other.transform.GetComponent<Player>();
            player.playerBuff.AddBuff(PlayerBuffType.Slow);
            Debug.Log(player);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (player != null)
        {
            player.playerBuff.DeleteBuff(PlayerBuffType.Slow);
            player = null;
        }
    }
    //public void CheckObj()
    //{
    //    Vector3 boxcenter = _col.bounds.center;
    //    Vector3 halfextents = _col.bounds.extents;

    //    isCheck = Physics.BoxCast(boxcenter, halfextents, transform.up, out hit, transform.rotation, rayDistance);
    //    if (isCheck)
    //    {
    //        Debug.Log("충돌함");
    //        if (hit.collider.gameObject.layer == 1 << LayerMask.NameToLayer("Player"))
    //        {
    //            Debug.Log("플레이어와 닿음");
    //            player = hit.transform.GetComponentInParent<Player>();
    //            player.playerBuff.AddBuff(PlayerBuffType.Slow);
    //    }
    //        }
    //    else
    //    {
    //            player.playerBuff.DeleteBuff(PlayerBuffType.Slow);
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (player != null)
    //    {
    //        player.playerBuff.DeleteBuff(PlayerBuffType.Slow);
    //        player = null;
    //    }
    //}
}
