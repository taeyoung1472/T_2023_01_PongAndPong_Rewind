using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterButtonUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _worldNameText = null;
    [SerializeField]
    private TextMeshProUGUI _chapterNameText = null;
    [SerializeField]
    private Image _chapterBackgroundImg;

    public void NameSet(StageWorldUI ui)
    {
        _worldNameText.SetText(ui.WorldName);
        _chapterNameText.SetText("ц╘ем " + ui.ChapterName);
        _chapterBackgroundImg.sprite = ui.chapterBackgroundSprite;
    }
}
