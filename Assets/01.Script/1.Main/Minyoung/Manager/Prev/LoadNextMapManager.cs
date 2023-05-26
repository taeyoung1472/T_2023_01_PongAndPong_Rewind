using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextMapManager : MonoBehaviour
{
    //private string SAVE_PATH = "Json";
    //private string SAVE_FILENAME = "/mapData.txt";

    //public TotalGimmickSO totalGimmickSO;
    //void Start()
    //{
    //    LoadMapData();
    //}

    //public void LoadMapData()
    //{
    //    //데이터 로드
    //    SAVE_PATH = Application.dataPath + "/Json";
    //    var loadJson = File.ReadAllText(SAVE_PATH + SAVE_FILENAME); //Resources.Load<TextAsset>("Json/mapData");
    //    MapData mapData = JsonUtility.FromJson<MapData>(loadJson);

    //    //생성
    //    for (int i = 0; i < mapData.pos.Count; i++)
    //    {
    //        GameObject obj = Instantiate(totalGimmickSO.gimmickInfo[mapData.index[i]].prefab, transform.position, Quaternion.identity);
    //        obj.transform.position = mapData.pos[i];
    //        obj.transform.rotation = mapData.rot[i];
    //        obj.transform.localScale = mapData.scale[i];
    //        Debug.Log($"{mapData.pos[i]}, {mapData.rot[i]}, {mapData.scale[i]}");
    //    }
    //}
    //public void GoDrawScene()
    //{
    //    SceneManager.LoadScene("MapJson");
    //}
}