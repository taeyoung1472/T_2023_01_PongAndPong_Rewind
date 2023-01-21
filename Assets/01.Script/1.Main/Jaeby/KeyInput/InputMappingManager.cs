using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputMappingManager : MonoSingleTon<InputMappingManager>
{
    KeyCode[] defaultKeys = new KeyCode[]
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

    [ContextMenu("로드!!")]
    public void LoadKey()
    {
        string path = Application.dataPath + "/Save/KeyData.json";
        string json = File.ReadAllText(path);
        KeyManager.keys = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<InputType, KeyCode>>(json); // json불러오기
    }

    public void SaveOneKey(InputType keyType, KeyCode value)
    {
        KeyManager.keys[keyType] = value;
    }

    [ContextMenu("저장!!")]
    public void SaveAllKey()
    {
        SaveOneKey(InputType.Jump, KeyCode.B);
        SaveOneKey(InputType.Attack, KeyCode.N);
        for (int i = 0; i < (int)InputType.Size; i++)
        {
            KeyManager.keys.TryAdd((InputType)i, defaultKeys[i]);
            //if (KeyManager.keys.ContainsKey((InputType)i) == false)
            //  KeyManager.keys.Add((InputType)i, defaultKeys[i]);
        }
        string path = Application.dataPath + "/Save/KeyData.json";
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(KeyManager.keys);
        File.WriteAllText(path, json);
    }

    [ContextMenu("키 리셋")]
    public void ResetKey()
    {
        KeyManager.keys.Clear();
        SaveAllKey();
    }
}
