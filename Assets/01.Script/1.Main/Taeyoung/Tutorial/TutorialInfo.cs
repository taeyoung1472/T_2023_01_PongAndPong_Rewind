using UnityEngine;

[CreateAssetMenu(menuName = "Data/TutoInfo")]
public class TutorialInfo : ScriptableObject
{
    public string tutoTitle, tutoSubTitle;
    [Header("NPC")]
    [TextArea]
    public string[] startNpcText;
    [TextArea]
    public string[] endNpcText;

    public StageDataSO stageData;
}
