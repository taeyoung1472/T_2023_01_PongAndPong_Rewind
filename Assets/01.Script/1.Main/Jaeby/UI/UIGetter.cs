using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGetter : MonoSingleTon<UIGetter>
{
    [SerializeField]
    private Transform _canvasTrm = null;

    [SerializeField]
    private GameObject _interactUI = null;
    private Image _interactImage = null;
    private TextMeshProUGUI _interactText = null;

    private void Start()
    {
        _interactImage = _interactUI.GetComponent<Image>();
        _interactText = _interactUI.GetComponentInChildren<TextMeshProUGUI>();
    }

    public GameObject GetInteractUI(Canvas canvas,Vector3 pos, Sprite sprite, KeyCode key)
    {
        _interactUI.transform.SetParent(canvas.transform);
        _interactUI.transform.position = pos;
        _interactImage.sprite = sprite;
        _interactText.SetText(key.ToString());

        _interactUI.SetActive(true);
        return _interactUI;
    }

    public void PushUIs()
    {
        _interactUI.transform.SetParent(_canvasTrm);
        _interactUI.transform.position = Vector3.zero;
        _interactUI.SetActive(false);
    }
}
