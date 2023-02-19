using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image _iconImage = null;
    public TextMeshProUGUI _itemNameText = null;
    private ItemData _data = null;
    public ItemData Data => _data;

    public void Init(ItemData data)
    {
        _data = data;
        _iconImage.sprite = data.icon;
        _itemNameText.SetText(_data.itemName);
    }
}
