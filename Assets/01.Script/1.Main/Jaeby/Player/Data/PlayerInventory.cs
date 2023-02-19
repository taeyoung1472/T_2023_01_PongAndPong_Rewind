using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private Dictionary<ItemType, List<string>> _inventory = new Dictionary<ItemType, List<string>>();
    public Dictionary<ItemType, List<string>> Inventory => _inventory;

    public void LoadInventory()
    {
        string json = File.ReadAllText(Application.dataPath + "/Save/Inventory.json");
        _inventory = JsonConvert.DeserializeObject<Dictionary<ItemType, List<string>>>(JsonUtility.ToJson(json));
        for (int i = 0; i < (int)ItemType.Size; i++)
        {
            _inventory[(ItemType)i] = new List<string>();
        }
    }

    public void SaveInventory()
    {
        string path = Application.dataPath + "/Save/Inventory.json";
        string json = JsonConvert.SerializeObject(_inventory);
        Debug.Log(json);
        File.WriteAllText(path, json);
    }

    public void AddItem(ItemData data)
    {
        List<string> temp;
        if(_inventory.TryGetValue(data.itemType, out temp) == false)
        {
            temp = new List<string>();
            _inventory[data.itemType] = temp;
        }
        temp.Add(data.itemKey);
    }
}
