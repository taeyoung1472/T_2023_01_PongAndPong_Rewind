using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoSingleTon<Shop>
{
    [SerializeField]
    private ItemSlot _slotPrefab = null;
    [SerializeField]
    private Transform _parent = null;

    private List<GameObject> _children = new List<GameObject>();

    public void ShopInit(List<string> itemKeys)
    {
        for (int i = 0; i < _children.Count; i++)
        {
            Destroy(_children[i]);
        }
        for (int i = 0; i < itemKeys.Count; i++)
        {
            ItemData data = ItemDB.Instance.TryGetItem(itemKeys[i]);
            if (data != null)
            {
                ItemSlot slot = Instantiate(_slotPrefab, _parent);
                slot.Init(data);
                _children.Add(slot.gameObject);
            }
        }
    }
}
