using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoSingleTon<ItemDB>
{
    [SerializeField]
    private List<ItemData> _itemDatas = new List<ItemData>();

    public ItemData TryGetItem(string key)
    {
        for(int i = 0; i < _itemDatas.Count; i++)
        {
            if(key == _itemDatas[i].itemKey)
            {
                return _itemDatas[i];
            }
        }
        return null;
    }
}
