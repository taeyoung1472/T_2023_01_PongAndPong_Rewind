using TMPro;
using UnityEngine;

public class OutLine_TMP : MonoBehaviour
{
    [Header("[Value]")]
    [SerializeField, Range(0f, 1f)] private float width = 0.2f;
    [SerializeField] private Color color;

    TextMeshPro myText;

    public void OnValidate()
    {
        if (myText == null)
        {
            myText = GetComponent<TextMeshPro>();
        }

        myText.outlineWidth = width;
        myText.outlineColor = color;
    }
}
