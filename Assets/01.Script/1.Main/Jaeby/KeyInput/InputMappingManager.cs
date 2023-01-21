using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputMappingManager : MonoSingleTon<InputMappingManager>
{
    [ContextMenu("로드!!")]
    public void LoadKey()
    {
        string path = Application.dataPath + "/Save/KeyData.json";
        string json = File.ReadAllText(path);
        KeyManager.keys = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<InputType, KeyCode>>(json); // json불러오기
    }
}
