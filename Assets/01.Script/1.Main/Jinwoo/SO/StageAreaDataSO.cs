using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SO/StageArea")]
public class StageAreaDataSO : ScriptableObject
{
    public StageAreaT areaPrefab;
    public ReTime reTimeObject;
    public int stagePlayTime;
    public bool isAreaClear = false;
}
