using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputMappingManager : MonoSingleTon<InputMappingManager>
{
    [ContextMenu("�ε�!!")]
    public void LoadKey()
    {
        string path = Application.dataPath + "/Save/KeyData.json";
        string json = File.ReadAllText(path);
        KeyManager.keys = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<InputType, KeyCode>>(json); // json�ҷ�����
    }
}
