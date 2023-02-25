using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StageUnitUI : MonoBehaviour
{
    private Sequence _seq = null;
    private Image _image = null;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public void UIAccent(Color endColor, float endSize, float duration)
    {
        if (_seq != null)
            _seq.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(transform.DOScale(endSize, duration));
        _seq.Join(_image.DOColor(endColor, duration));
    }

    public void AccectReset()
    {
        if(_seq != null)
            _seq.Kill();
        transform.localScale = Vector3.one;
        _image.color = Color.white;
    }
}
