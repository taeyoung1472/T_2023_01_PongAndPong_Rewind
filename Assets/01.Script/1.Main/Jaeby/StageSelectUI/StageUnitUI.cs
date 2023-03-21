using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StageUnitUI : MonoBehaviour
{
    private Sequence _seq = null;
    private Image _image = null;
    [SerializeField]
    private StageInfoSO _stageInfoSO = null;
    public StageInfoSO stageInfoSO => _stageInfoSO;

    public void UIAccent(Color endColor, float endSize, float duration)
    {
        if (_seq != null)
            _seq.Kill();
        if (_image == null)
            _image = GetComponent<Image>();
        _seq = DOTween.Sequence();
        _seq.Append(transform.DOScale(endSize, duration));
        _seq.Join(_image.DOColor(endColor, duration));
    }

    public void AccectReset()
    {
        if(_seq != null)
            _seq.Kill();
        transform.localScale = Vector3.one;
        if (_image == null)
            _image = GetComponent<Image>();
        _image.color = Color.white;
    }
}
