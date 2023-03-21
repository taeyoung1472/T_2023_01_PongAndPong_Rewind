using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SO/StageArea")]
public class StageAreaDataSO : ScriptableObject
{
    public string areaInfo;
    public int stagePlayTime;
    public bool isAreaClear = false;
}
