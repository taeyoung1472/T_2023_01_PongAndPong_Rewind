using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommunicationUIPrefab : MonoBehaviour
{
    [SerializeField]
    private Vector2 _textBoxExpendAmount = Vector2.zero;

    private CanvasGroup _canvasGroup = null;
    private Image _image = null;
    private TextMeshProUGUI _content = null;
    private Image _textBox = null;
    private Animator _faceImageAnimator = null;
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
        Vector2 textBoxPosition = (Vector2)_content.rectTransform.localPosition + textSize * 0.5f;
        _textBox.rectTransform.localPosition = textBoxPosition;
        _textBox.rectTransform.sizeDelta = textSize + _textBoxExpendAmount;
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
        _faceImageAnimator = _image.GetComponent<Animator>();
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
        _faceImageAnimator.SetBool("Talk", true);
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
        _faceImageAnimator.SetBool("Talk", false);
    }
}
