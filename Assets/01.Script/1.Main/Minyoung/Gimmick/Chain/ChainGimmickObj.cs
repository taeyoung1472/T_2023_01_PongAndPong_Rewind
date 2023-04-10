using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainGimmickObj : GimmickObject
{
    ChainGimmick chainGimmick;
    List<ChainGimmickObj> chainGimmickObjList = new List<ChainGimmickObj>();
    private void Awake()
    {
        Init();
    }
    public override void Init()
    {
        chainGimmick = FindObjectOfType<ChainGimmick>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            int index = chainGimmick.chainList.FindIndex(x => x == this);
            Debug.Log(index);
            //if (index == chainGimmick.chainList.Count - 2)
            //{
            //    chainGimmick.chainList[index].GetComponent<Rigidbody>().useGravity = true;
            //    chainGimmick.chainList[index].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            //}
            //else
            //{
            for (int i = index; i < chainGimmick.chainList.Count; i++)
            {
                chainGimmick.chainList[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                chainGimmick.chainList[i].GetComponent<Rigidbody>().freezeRotation = true;
                //chainGimmick.chainList[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                chainGimmick.chainList[i].GetComponent<Rigidbody>().useGravity = true;
            }
        //}
          
            chainGimmick.chainList.Remove(this);

            Destroy(gameObject);
        }
        //ºÒ·¿°ú¸ÂÀ¸¸é »ç¶óÁü       
    }
}
