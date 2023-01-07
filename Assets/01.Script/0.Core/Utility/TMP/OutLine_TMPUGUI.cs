using TMPro;
using UnityEngine;

public class OutLine_TMPUGUI : MonoBehaviour
{
    [Header("[Value]")]
    [SerializeField, Range(0f, 1f)] private float width = 0.2f;
    [SerializeField] private Color color = Color.black;

    TextMeshProUGUI myTextUGUI;

    public void OnValidate()
    {
        if (myTextUGUI == null)
        {
            myTextUGUI = GetComponent<TextMeshProUGUI>();
        }

        myTextUGUI.outlineWidth = width;
        myTextUGUI.outlineColor = color;
    }
}
