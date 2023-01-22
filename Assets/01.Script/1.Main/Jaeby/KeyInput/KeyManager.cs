using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class KeyManager
{
    public static KeyCode[] defaultKeys = new KeyCode[]
    {
        KeyCode.W,
        KeyCode.S,
        KeyCode.A,
        KeyCode.D,
        KeyCode.Space,
        KeyCode.Mouse0,
        KeyCode.LeftShift,
        KeyCode.Mouse1,
    };

    public static Dictionary<InputType, KeyCode> keys = new Dictionary<InputType, KeyCode>();

    public static void LoadKey()
    {
        string path = Application.dataPath + "/Save/KeyData.json";
        string json = File.ReadAllText(path);
        keys = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<InputType, KeyCode>>(json); // json불러오기
        DefaultKeyAdd();
    }

    public static void ChangeKeySetting(Dictionary<InputType, KeyCode> newKeys)
    {
        foreach(var k in newKeys)
            keys[k.Key] = k.Value;

        foreach(var k in keys)
        {
            Debug.Log(k);
        }
    }

    public static void DefaultKeyAdd()
    {
        for (int i = 0; i < (int)InputType.Size; i++)
        {
            keys.TryAdd((InputType)i, defaultKeys[i]);
        }
    }
}
