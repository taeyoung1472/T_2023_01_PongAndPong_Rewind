using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stage/InGameInfo")]
public class StageDataSO : ScriptableObject
{
    public string stageName;
    public int stageNumber;
    public bool isClear = false;
    public Stage stagePrefab;
    public StageDataSO nextStageData;

    public string stageInfo;

    public Sprite stageSprite;
    public Sprite closerStageSprite;

    public string stageSubTitle;
    public string stageExplain;
    public int stageIndex;


    //¼öÁýÇ°
    public List<bool> stageCollection;
}
