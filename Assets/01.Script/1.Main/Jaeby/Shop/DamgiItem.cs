using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamgiItem : MonoBehaviour
{
    [SerializeField]
    private Image _itemImage = null;
    [SerializeField]
    private TextMeshProUGUI _itemNameText = null;
    [SerializeField]
    private TextMeshProUGUI _moneyText = null;

    public void UISet(ItemData data)
    {
        _itemImage.sprite = data.icon;
        _itemNameText.SetText(data.itemName);
        _moneyText.SetText(data.price.ToString());
    }
}
