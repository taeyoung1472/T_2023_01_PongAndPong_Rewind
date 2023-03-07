using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stage")]
public class StageDataSO : ScriptableObject
{
    public List<StageAreaT> StageAreaList;
    //public StageT stagePrefab;
    public bool isClear = false;
    public string stageInfo;
}
