using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoSingleTon<SaveManager>
{
    //private string SAVE_PATH = "Json";
    //private string SAVE_FILENAME = "/mapData.txt";

    //public List<TransformInfo> transformList;
    //public MapData mapData;

    //public TotalGimmickSO totalGimmickSO;
    //public void Save()
    //{
    //    mapData.pos.Clear();
    //    mapData.rot.Clear();
    //    mapData.scale.Clear();
    //    mapData.index.Clear();

    //    foreach (var info in transformList)
    //    {
    //        mapData.pos.Add(info.transform.position);
    //        mapData.rot.Add(info.transform.rotation);
    //        mapData.scale.Add(info.transform.localScale);
    //        mapData.index.Add(info.index);
    //    }

    //}

    //public void SaveTransfomDataToJson()
    //{
    //    SAVE_PATH = Application.dataPath + "/Json";
    //    if (Directory.Exists(SAVE_PATH) == false)
    //    {
    //        Directory.CreateDirectory(SAVE_PATH);
    //    }
    //    string jsonData = JsonUtility.ToJson(mapData);
    //    File.WriteAllText(SAVE_PATH + SAVE_FILENAME, jsonData); //텍스트에셋을 만드는거야
    //}
    
    //public void TestMap()
    //{
    //    SceneManager.LoadScene("MakeMapTestScene");
    //}

    //public void CreateSaveObj()
    //{
    //    SAVE_PATH = Application.dataPath + "/Json";
    //    var loadJson = File.ReadAllText(SAVE_PATH + SAVE_FILENAME); //Resources.Load<TextAsset>("Json/mapData");
    //    MapData mapObjData = JsonUtility.FromJson<MapData>(loadJson);

    //    for (int i = 0; i < mapObjData.pos.Count; i++)
    //    {
    //        GameObject obj = Instantiate(totalGimmickSO.gimmickInfo[mapObjData.index[i]].prefab, transform.position, Quaternion.identity);
    //        obj.transform.position = mapObjData.pos[i];
    //        obj.transform.rotation = mapObjData.rot[i];
    //        obj.transform.localScale = mapObjData.scale[i];
    //        Debug.Log($"{mapObjData.pos[i]}, {mapObjData.rot[i]}, {mapObjData.scale[i]}");
    //    }
    //}
}
