using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogOptionUI : MonoBehaviour
{
    [SerializeField]
    private Image _iconImage = null;
    [SerializeField]
    private TextMeshProUGUI _explainText = null;
    [SerializeField]
    private Button _button = null;

    public void Init(Sprite icon, string explain, UnityAction onclick)
    {
        _iconImage.sprite = icon;
        _explainText.SetText(explain);
        _button.onClick.AddListener(onclick);
    }
}
