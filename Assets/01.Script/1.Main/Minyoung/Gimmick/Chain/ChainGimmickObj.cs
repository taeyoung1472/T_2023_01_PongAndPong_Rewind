using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainGimmickObj : GimmickObject, IDamageable
{
    ChainGimmick chainGimmick;
    List<ChainGimmickObj> chainGimmickObjList = new List<ChainGimmickObj>();
    public override void Awake()
    {
        base.Awake();
        Init();
    }

    public void Damaged(ColliderType colliderType)
    {
        if (colliderType == ColliderType.PlayerBullet)
        {
            int index = chainGimmick.chainList.FindIndex(x => x == this);
            Debug.Log(index);
            for (int i = index; i < chainGimmick.chainList.Count; i++)
            {
                chainGimmick.chainList[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                chainGimmick.chainList[i].GetComponent<Rigidbody>().freezeRotation = true;
                chainGimmick.chainList[i].GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }

    public override void Init()
    {
        chainGimmick = FindObjectOfType<ChainGimmick>();
    }

    private void OnTriggerEnter(Collider other)
    {
    }
}
