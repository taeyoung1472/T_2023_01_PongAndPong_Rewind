using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class StageUnitUI : MonoBehaviour
{
    private Sequence _seq = null;
    [SerializeField]
    private Image _iconImage = null;
    [SerializeField]
    private Image _glowImage = null;
    [SerializeField]
    private StageDataSO _stageDataSO = null;
    public StageDataSO StageDataSO => _stageDataSO;

    private Animator _animator = null;

    private void Awake()
    {
        _animator = _iconImage.GetComponent<Animator>();
    }

    public void UIAccent(Color endColor, float endSize, float duration, bool animationLoop = false)
    {
        if (_seq != null)
            _seq.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(transform.DOScale(endSize, duration));
        _seq.Join(_iconImage.DOColor(endColor, duration));
        _seq.Join(_glowImage.DOColor(endColor, duration));
        _animator.SetBool("Loop", animationLoop);
    }

    public void AccectReset()
    {
        if(_seq != null)
            _seq.Kill();
        transform.localScale = Vector3.one;
        _iconImage.color = Color.white;
        _glowImage.color = Color.white;
        _animator.SetBool("Loop", false);
    }

    public void SetStageData()
    {
        StageManager.stageDataSO = _stageDataSO;
    }
}
