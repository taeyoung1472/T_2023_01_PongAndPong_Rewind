using UnityEngine;

[CreateAssetMenu(menuName = "Data/TutoInfo")]
public class TutorialInfo : ScriptableObject
{
    public string tutoTitle, tutoSubTitle;
    public string tutoStartText;
    public string tutoEndText;

    public StageDataSO stageData;
}
