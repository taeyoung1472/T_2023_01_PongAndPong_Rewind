using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatVisual : MonoBehaviour
{
    [Header("[Main]")]
    [SerializeField] private TextMeshProUGUI nicknameTMP;
    [SerializeField] private Image profileImage;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject newBar;

    [Header("[Template]")]
    [SerializeField] private TextMeshProUGUI textTemplate;
    [SerializeField] Image imageTemplate;

    public void Set(ChatUnit unit, ChatDataSO data, int index)
    {
        nicknameTMP.SetText(data.myInfo.ToString());
        profileImage.sprite = data.profile;

        if(data.curChatIndex == index)
        {
            newBar.gameObject.SetActive(true);
        }

        foreach (var content in unit.chatContents)
        {
            if(content.text != string.Empty)
            {
                TextMeshProUGUI tmp = Instantiate(textTemplate, contentParent);
                tmp.SetText(content.text);
                tmp.gameObject.SetActive(true);
            }

            if (content.image != null)
            {
                Image img = Instantiate(imageTemplate, contentParent);
                img.sprite = content.image;
                img.SetNativeSize();
                img.gameObject.SetActive(true);
            }
        }
        contentParent.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }
}
