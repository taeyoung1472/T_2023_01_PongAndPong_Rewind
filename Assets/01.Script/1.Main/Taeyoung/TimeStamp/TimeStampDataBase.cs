using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "DB/Stamp")]
public class TimeStampDataBase : ScriptableObject
{
    public StampData[] stampDataArr;
    private Dictionary<StampType, StampData> stampDic = new();

    public void GenDic()
    {
        for (int i = 0; i < stampDataArr.Length; i++)
        {
            stampDic.Add(stampDataArr[i].type, stampDataArr[i]);
        }
    }

    public StampData GetData(StampType type)
    {
        return stampDic[type];
    }

    public void OnValidate()
    {
        stampDataArr = stampDataArr.OrderBy(a => a.type).ToArray();
        for (int i = 0; i < stampDataArr.Length; i++)
        {
            stampDataArr[i].name = $"{(int)stampDataArr[i].type}_{stampDataArr[i].type}";
        }
    }

    [System.Serializable]
    public class StampData
    {
        [HideInInspector] public string name;
        public StampType type;
        public Sprite sprite;
        public Color stampColor = Color.white;
    }
}

public enum StampType
{
    doorOpen = 100,
    doorClose = 101,
    grabityChange = 200,
    dropBox = 300,
    killEnemy = 400
}