using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SO/World/worldData")]
public class WorldDataSO : ScriptableObject
{
    public string worldName;
    public WorldType worldType;
    public List<StageDataSO> stageList;

    public StageDataSO GetStage(int index)
    {
        return stageList[index];
    }
}
