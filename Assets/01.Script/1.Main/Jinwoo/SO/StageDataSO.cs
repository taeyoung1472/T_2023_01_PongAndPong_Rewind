using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stage/InGameInfo")]
public class StageDataSO : ScriptableObject
{
    public string stageName;
    public int stageNumber;
    public bool isClear = false;

    public string stageInfo;
}
