using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notify : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RectTransform content;
    [SerializeField] private AudioClip open;
    [SerializeField] private AudioClip close;
    private RectTransform myRect;


    public void SetNotify(string notifyContent)
    {
        myRect = GetComponent<RectTransform>();
        text.SetText(notifyContent);
        content.anchoredPosition = new Vector2(-99999, -99999);
        AudioManager.PlayAudio(open);
        StartCoroutine(RegenRect());
    }

    IEnumerator RegenRect()
    {
        yield return null;
        text.gameObject.SetActive(true);
        yield return null;
        content.anchoredPosition = new Vector2(content.sizeDelta.x * 1.5f, -content.sizeDelta.y);
        DOTween.To(() => myRect.sizeDelta, x => myRect.sizeDelta = x, new Vector2(myRect.sizeDelta.x, 75), 0.4f);
        DOTween.To(() => content.anchoredPosition, x => content.anchoredPosition = x, Vector2.zero, 0.75f);
    }

    public void Close()
    {
        Sequence seq = DOTween.Sequence();

        AudioManager.PlayAudio(close);
        seq.Append(DOTween.To(() => content.anchoredPosition, x => content.anchoredPosition = x, new Vector2(content.sizeDelta.x * 1.5f, 0), 0.75f));
        seq.AppendCallback(() => Destroy(gameObject));
    }
}
