using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainGimmick : MonoBehaviour
{
    public List<ChainGimmickObj> chainList = new List<ChainGimmickObj>();
    void Start()
    {
        foreach (var item in transform.GetComponentsInChildren<ChainGimmickObj>())
        {
            chainList.Add(item);
        }
    }
}
