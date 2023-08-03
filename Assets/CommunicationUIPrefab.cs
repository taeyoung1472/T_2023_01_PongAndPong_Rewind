using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommunicationUIPrefab : MonoBehaviour
{
    private CanvasGroup _canvasGroup = null;
    private Image _image = null;
    private TextMeshProUGUI _content = null;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _image = GetComponentInChildren<Image>();
        _content = GetComponent<TextMeshProUGUI>();
    }

    public void SetUI(Sprite sprite, string content)
    {
        _image.enabled = sprite != null;
        _image.sprite = sprite;
        _content.text = content;
    }
}
