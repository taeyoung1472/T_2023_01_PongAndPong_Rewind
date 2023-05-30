using TMPro;
using UnityEngine;

public class TimerText : MonoBehaviour
{
    TextMeshProUGUI text;
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        TimerManager.Instance.OnTimeChange += SetText;
    }

    void SetText(float t)
    {
        text.SetText($"{t:0.0}");
    }
}
