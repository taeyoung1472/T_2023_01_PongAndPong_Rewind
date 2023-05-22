using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stage/InGameInfo")]
public class StageDataSO : ScriptableObject
{
    public string stageName;
    [HideInInspector] public int stageNumber;
    [HideInInspector] public bool isClear = false;
    public Stage stagePrefab;
    public StageDataSO nextStageData;

    //public string stageInfo;
    public Sprite stageSprite;
    public Sprite closerStageSprite;

    [HideInInspector] public int stageIndex;
    public TutorialInfo tutorialInfo;

    //¼öÁýÇ°
    public List<bool> stageCollection;
}
