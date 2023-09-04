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
    //����ǰ
    public List<BoolList> stageCollection; 

    //�� UI ������ �ʿ���
    public List<GimmickInfoSO> useGimmickStageList;

}

[System.Serializable]
public class BoolList
{
    public List<bool> zone;
}