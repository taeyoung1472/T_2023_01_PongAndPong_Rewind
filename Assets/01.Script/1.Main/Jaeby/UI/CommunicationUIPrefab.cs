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
    private Image _textBox = null;
    private Sequence _animationSeq = null;
    private Coroutine _textAnimationCoroutine = null;

    public void SetUI(Sprite sprite, string content)
    {
        SettingComponent();

        _image.enabled = sprite != null;
        _image.sprite = sprite;
        _content.text = content;
        _content.ForceMeshUpdate();
        Vector2 textSize = _content.GetRenderedValues();
        _textBox.rectTransform.sizeDelta = textSize;
        Vector2 anchoredPos = _content.rectTransform.anchoredPosition;
        anchoredPos.y += textSize.y;
        _content.rectTransform.anchoredPosition = anchoredPos;
        _content.text = "";
        _textAnimationCoroutine = StartCoroutine(TextAnimationCoroutine(content));
    }

    private void SettingComponent()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _content = GetComponentInChildren<TextMeshProUGUI>();
        _image = transform.Find("CharacterSprite").GetComponent<Image>();
        _textBox = transform.Find("TextBox").GetComponent<Image>();
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

    private void OnDestroy()
    {
        if (_textAnimationCoroutine != null)
        {
            StopCoroutine(_textAnimationCoroutine);
        }
    }

    private IEnumerator TextAnimationCoroutine(string endText)
    {
        string text = "";
        for(int i = 0; i < endText.Length; i++)
        {
            if (endText[i] == '@')
            {
                text += "\n";
            }
            else
            {
                text += endText[i];
            }
            _content.text = text;
            _content.ForceMeshUpdate();
            yield return new WaitForSeconds(0.05f);
        }
    }
}
