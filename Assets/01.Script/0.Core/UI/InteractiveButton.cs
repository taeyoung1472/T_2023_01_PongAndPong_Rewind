using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class InteractiveButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Color activeColor = Color.white;
    [SerializeField] protected float activeMoveValue = 0;
    [SerializeField] protected float activeSizeValue = 1;
    [SerializeField] protected AudioClip activeClip;
    protected Image image;
    protected RectTransform rectTransform;
    protected Color originColor;
    protected float originPos;
    public virtual void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        originPos = rectTransform.anchoredPosition.x;
        originColor = image.color;
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Kill();

        rectTransform.DOAnchorPosX(originPos + activeMoveValue, 0.25f).SetUpdate(true);
        rectTransform.DOScale(activeSizeValue, 0.2f).SetUpdate(true);
        image.DOColor(activeColor, 0.2f).SetUpdate(true);
        if(activeClip != null)
        {
            AudioManager.PlayAudioRandPitch(activeClip);
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Kill();

        rectTransform.DOAnchorPosX(originPos, 0.5f).SetUpdate(true);
        rectTransform.DOScale(1, 0.25f).SetUpdate(true);
        image.DOColor(originColor, 0.4f).SetUpdate(true);
    }

    private void OnDisable()
    {
        Kill();

        rectTransform.anchoredPosition = new Vector2(originPos, rectTransform.anchoredPosition.y);
        rectTransform.localScale = Vector3.one;
        image.color = originColor;
    }

    protected virtual void Kill()
    {
        image.DOKill();
        rectTransform.DOKill();
    }
}
