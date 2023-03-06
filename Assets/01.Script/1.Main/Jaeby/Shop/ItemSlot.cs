using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private Image _iconImage = null;
    [SerializeField]
    private TextMeshProUGUI _itemNameText = null;
    [SerializeField]
    private TextMeshProUGUI _itemPriceText = null;
    private ItemData _data = null;
    public ItemData Data => _data;

    public void Init(ItemData data)
    {
        _data = data;
        _iconImage.sprite = data.icon;
        _itemNameText.SetText(data.itemName);
        _itemPriceText.SetText(data.price.ToString());
    }

    public void TryBuy()
    {
        if(_data.price <= player.playerJsonData.money)
        {
            player.playerJsonData.money -= _data.price;
            player.playerInventory.AddItem(_data);
            player.SaveJsonData();
        }
    }

    public void TryDamgi()
    {
        if (ShopManager.Instance.TryDamgi(this) == false)
        {
            Debug.LogError("쓸 수 없어요 ㅠㅠ");
            return;
        }
    }
}
