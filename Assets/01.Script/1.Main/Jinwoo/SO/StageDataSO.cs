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
    public bool isFirst;

    //public string stageInfo;
    public Sprite stageSprite;
    public Sprite closerStageSprite;

    public int stageIndex;

    public string chapterStageName;
    //수집품
    public List<BoolList> stageCollection; 

    //폰 UI 때문에 필요함
    public List<GimmickInfoSO> useGimmickStageList;

}

[System.Serializable]
public class BoolList
{
    public List<bool> zone;
}