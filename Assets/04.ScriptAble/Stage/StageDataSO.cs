using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stage")]
public class StageDataSO : ScriptableObject
{
    public int stagePlayTime;
    public GameObject StagePrefab;
    public bool isClear = false;
}
