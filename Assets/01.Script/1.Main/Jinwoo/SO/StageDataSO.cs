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
    //����ǰ
    public List<bool> stageCollection;

    //�� UI ������ �ʿ���
    public List<GimmickInfoSO> useGimmickStageList;
}
