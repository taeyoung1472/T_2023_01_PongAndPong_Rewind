using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrentStageNameData
{
    public string worldName;
    public int currentStageIndex;
    public int stageCnt;

    public Dictionary<string, bool> cutSceneDic = new Dictionary<string, bool>();

}
