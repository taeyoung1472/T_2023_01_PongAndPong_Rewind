using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Shop/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemKey;
    public ItemType itemType;
    public Sprite icon;
    public string itemName;
    public string explain;
    public int price;
}
