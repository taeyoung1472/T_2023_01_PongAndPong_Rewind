using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : MonoBehaviour
{
    [SerializeField]
    private List<ItemData> _haveItems = new List<ItemData>();

    public void ShopInit()
    {
        Shop.Instance.ShopInit(_haveItems);
    }
}
