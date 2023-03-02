using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SO/StageArea")]
public class StageAreaDataSO : ScriptableObject
{
    public GameObject areaPrefab;
    public int stagePlayTime;
    public bool isAreaClear = false;
}
