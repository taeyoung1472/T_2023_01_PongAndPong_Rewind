using DG.Tweening;
using System;
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
    private Sequence _animationSeq = null;

    public void SetUI(Sprite sprite, string content)
    {
        SettingComponent();

        _image.enabled = sprite != null;
        _image.sprite = sprite;
        _content.text = content;
    }

    private void SettingComponent()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _image = GetComponentInChildren<Image>();
        _content = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void AnimationUI(float startValue, float endValue, float animationTime, Action Callback = null)
    {
        if (_animationSeq != null)
        {
            _animationSeq.Kill();
        }
        _canvasGroup.alpha = startValue;
        _animationSeq = DOTween.Sequence();
        _animationSeq.Append(_canvasGroup.DOFade(endValue, animationTime));
        _animationSeq.AppendCallback(() =>
        {
            _animationSeq.Kill();
            Callback?.Invoke();
        });
    }
}
