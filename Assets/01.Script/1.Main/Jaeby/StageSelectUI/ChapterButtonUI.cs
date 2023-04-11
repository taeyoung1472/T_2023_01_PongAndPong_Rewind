using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChapterButtonUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _worldNameText = null;
    [SerializeField]
    private TextMeshProUGUI _chapterNameText = null;

    public void NameSet(StageWorldUI ui)
    {
        _worldNameText.SetText(ui.WorldName);
        _chapterNameText.SetText("ц╘ем " + ui.ChapterName);
    }
}
