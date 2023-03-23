using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class StageUnitUI : MonoBehaviour
{
    private Sequence _seq = null;
    private Image _image = null;
    [SerializeField]
    private StageDataSO _stageDataSO = null;
    public StageDataSO StageDataSO => _stageDataSO;

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

    public void SetStageData()
    {
        StageManager.stageDataSO = _stageDataSO;
    }
}
