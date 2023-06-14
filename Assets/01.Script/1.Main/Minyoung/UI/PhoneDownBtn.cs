using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
public class PhoneDownBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        DOTween.Kill(this);
        transform.GetComponent<Image>().DOFade(1f, 0.3f).SetUpdate(true);
        transform.GetComponent<RectTransform>().DOSizeDelta(new Vector2(100, 55), 0.3f).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DOTween.Kill(this);
        transform.GetComponent<Image>().DOFade(0f, 0.3f).SetUpdate(true).OnComplete(() =>
        {
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 55f);
        });

    }
}
