using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class BackgroundBox : MonoBehaviour
{
    [SerializeField] private RectTransform targetText;
    [SerializeField] private float edgeVoid;
    private RectTransform myRect;

    private void Start()
    {
        myRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        myRect.sizeDelta = targetText.sizeDelta + (Vector2.one * edgeVoid);
    }
}
