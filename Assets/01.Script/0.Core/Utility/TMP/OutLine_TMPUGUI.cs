using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutLine_TMPUGUI : MonoBehaviour
{
    [Header("[Value]")]
    [SerializeField, Range(0f, 0.7f)] private float width = 0.2f;
    [SerializeField] private Color color = Color.black;

    TextMeshProUGUI myText;

    public void OnValidate()
    {
        if (myText == null)
        {
            myText = GetComponent<TextMeshProUGUI>();
        }

        myText.outlineWidth = width;
        myText.outlineColor = color;
    }
}
