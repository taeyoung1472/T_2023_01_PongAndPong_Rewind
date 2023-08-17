using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutoPlayTextAnim : MonoBehaviour
{
    [SerializeField]
    private List<AutoPlayTextAnimData> _autoPlayTextAnimDatas = new List<AutoPlayTextAnimData>();
    [SerializeField]
    private float _nextSpeechTime = 1f;
    [SerializeField]
    private float _fadeAnimationTime = 0.2f;

    private AutoPlayTextAnimData _currentTextAnimData = null;
    private int _currentIndex = 0;
    private int _currnetTextIndex = 0;
    private Sequence _animationSeq = null;

    [ContextMenu("½ºÆ®Å×")]
    public void ASEASD()
    {
        _currentIndex = 0;
        _currnetTextIndex = 0;
        StartTextAnim();
    }

    public void StartTextAnim()
    {
        if (_currentIndex > _autoPlayTextAnimDatas.Count - 1)
            return;

        _currentTextAnimData = _autoPlayTextAnimDatas[_currentIndex];
        StartSettingText();
    }

    private void TextDataChange()
    {
        if (_animationSeq != null)
            _animationSeq.Kill();
        _animationSeq = DOTween.Sequence();
        _animationSeq.Append(_currentTextAnimData.speechBubble.DOFade(0f, _fadeAnimationTime));
        _animationSeq.AppendCallback(() =>
        {
            _currentTextAnimData.textMeshPro.gameObject.SetActive(false);
            _currnetTextIndex = 0;
            _currentIndex++;
        });
        _animationSeq.AppendInterval(_nextSpeechTime);
        _animationSeq.AppendCallback(() =>
        {
            StartTextAnim();
        });
    }

    private void StartSettingText()
    {
        if (_animationSeq != null)
            _animationSeq.Kill();
        _animationSeq = DOTween.Sequence();
        _animationSeq.Append(_currentTextAnimData.speechBubble.DOFade(1f, _fadeAnimationTime));
        _animationSeq.AppendCallback(() =>
        {
            StartCoroutine(TextAnimation(NextTextCheck));
        });
    }

    private void NextTextCheck()
    {
        if (_currnetTextIndex >= _currentTextAnimData.textData.stringArray.Length - 1)
        {
            TextDataChange();
        }
        else
        {
            _currnetTextIndex++;
            StartSettingText();
        }
    }

    private IEnumerator TextAnimation(Action Callback)
    {
        _currentTextAnimData.textMeshPro.gameObject.SetActive(true);
        _currentTextAnimData.textMeshPro.SetText(_currentTextAnimData.textData.stringArray[_currnetTextIndex]);
        _currentTextAnimData.textMeshPro.ForceMeshUpdate();
        int counter = 0;
        int totalVisibleCharacters = _currentTextAnimData.textMeshPro.textInfo.characterCount;
        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            _currentTextAnimData.textMeshPro.maxVisibleCharacters = visibleCount;
            if (visibleCount >= totalVisibleCharacters)
                break;
            counter += 1;

            AudioManager.PlayAudioRandPitch(SoundType.OnNPCSpeak);
            yield return new WaitForSeconds(_currentTextAnimData.textData.timeBtwnChars);
        }

        yield return new WaitForSeconds(_currentTextAnimData.textData.timeBtwnWords);
        Callback?.Invoke();
    }
}

[System.Serializable]
public class AutoPlayTextAnimData
{
    public TextMeshProUGUI textMeshPro;
    public TextAnimDataSO textData;
    public Image speechBubble;
}