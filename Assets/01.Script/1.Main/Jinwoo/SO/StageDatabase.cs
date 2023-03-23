using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/StageDB")]
public class StageDatabase : ScriptableObject
{
    public List<WorldDataSO> worldList;

    public WorldDataSO GetWorldData(WorldType worldType)
    {
        WorldDataSO returnValue = null;

        foreach (var world in worldList)
        {
            if(world.worldType == worldType)
            {
                returnValue = world;
                break;
            }
        }

        return returnValue;
    }
}
