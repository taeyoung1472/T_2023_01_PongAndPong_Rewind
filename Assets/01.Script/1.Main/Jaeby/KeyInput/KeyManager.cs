using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class KeyManager
{
    public static Dictionary<InputType, KeyCode> keys = new Dictionary<InputType, KeyCode>();

    public static void LoadKey()
    {
        string path = Application.dataPath + "/Save/KeyData.json";
        string json = File.ReadAllText(path);
        KeyManager.keys = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<InputType, KeyCode>>(json); // json불러오기
    }
}
