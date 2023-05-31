using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SO/World/worldData")]
public class WorldDataSO : ScriptableObject
{
    public string worldName;
    public WorldType worldType;
    public List<StageDataSO> stageList;

    public int rewardCount = 10;
    public string collectObjectName = "";
    public GameObject collectObject = null;


    public List<RewardFunctionData> _rewardFunctionData = new List<RewardFunctionData>();

    public StageDataSO GetStage(int index)
    {
        return stageList[index];
    }

    public string GetFunctionName(int count)
    {
        foreach(var a in _rewardFunctionData)
        {
            if (count >= a.targetCount)
                return a.function;
        }
        return null;
    }

    private void OnValidate()
    {
        for (int i = 0; i < stageList.Count; i++)
        {
            stageList[i].stageNumber = i + 1;
            stageList[i].stageIndex = i;
        }
    }
}

[System.Serializable]
public class RewardFunctionData
{
    public int targetCount = 0;
    public string function = "";
}
