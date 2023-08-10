using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicUITestJaeby : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;

    [ContextMenu("업데이트 ")]
    public void TextUpdate()
    {
    }

    private void Update()
    {
        Vector2 size = text.GetRenderedValues();
        Debug.Log(size);
        image.rectTransform.sizeDelta = size;
    }
}
