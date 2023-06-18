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

    public int stageIndex;
    public TutorialInfo tutorialInfo;

    public string chapterStageName;
    //수집품
    public List<bool> stageCollection;

    //폰 UI 때문에 필요함
    public List<GimmickInfoSO> useGimmickStageList;
}
