using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/StageSelectScene/StageWorldList")]
public class StageWorldListSO : ScriptableObject
{
    public List<StageWorld> stageWorlds = new List<StageWorld>();
}
