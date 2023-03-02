using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stage")]
public class StageDataSO : ScriptableObject
{
    public List<StageAreaDataSO> StageAreaPrefab;
    public bool isClear = false;
    public string stageInfo;
}
