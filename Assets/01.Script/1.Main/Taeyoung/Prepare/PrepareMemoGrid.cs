using UnityEngine;
using UnityEngine.UI;

public class PrepareMemoGrid : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Outline outline;
    [SerializeField] private Button button;

    private PrepareMemoData data;
    public PrepareMemoData Data { get { return data; } }

    public void Init(PrepareMemoData data, PrepareManager manager)
    {
        this.data = data;
        iconImage.sprite = data.icon;
        button.onClick.AddListener(() => manager.OnClickGrid(this));
    }

    public void FocusGrid(bool isFocus)
    {
        if (isFocus)
            outline.effectColor = Color.red;
        else
            outline.effectColor = Color.black;
    }
}
