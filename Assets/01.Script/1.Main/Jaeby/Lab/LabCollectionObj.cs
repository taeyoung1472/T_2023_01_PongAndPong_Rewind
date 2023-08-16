using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LabCollectionObj : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _percentText = null;
    private DissolveAnimator _dissolveAnimator = null;

    [Header("테스트용")]
    public float _textAnimatingTime = 1f;
    public int targetPercent = 46;

    public void CollectPercentSet()
    {
        if (_dissolveAnimator == null)
            _dissolveAnimator = GetComponent<DissolveAnimator>();
        _dissolveAnimator.DissolveStart(0f, 1f, new Vector3(0f, 1f, 0f));
        TextAnimating();
    }

    private void TextAnimating()
    {
        StartCoroutine(TextAnimatingCoroutine());
    }

    private IEnumerator TextAnimatingCoroutine()
    {
        float targetPercent = ((float)3 / 7) * 100f;
        float time = 0f;
        while (time <= 1f)
        {
            _percentText.SetText($"{(targetPercent * time).ToString("N0")}%");
            time += Time.deltaTime * (1 / _textAnimatingTime);
            yield return null;
        }
        _percentText.SetText($"{(targetPercent).ToString("N0")}%");
    }
}
