using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : MonoBehaviour
{
    [SerializeField]
    private List<string> _haveItems = new List<string>();

    public void ShopInit()
    {
        Shop.Instance.ShopInit(_haveItems);
    }
}
