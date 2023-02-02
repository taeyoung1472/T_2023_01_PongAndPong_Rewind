using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class InteractiveButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Color originColor = Color.white;
    [SerializeField] protected Color activeColor = Color.white;
    [SerializeField] protected float activeMoveValue = 0;
    [SerializeField] protected float activeSizeValue = 1;
    [SerializeField] protected AudioClip hoverClip;
    [SerializeField] protected AudioClip clickClip;
    [SerializeField] protected bool isMove = false;

    [Header("Materials")]
    [SerializeField] private TextMeshProUGUI[] textList;
    [SerializeField] private Image[] imageList;

    protected RectTransform rectTransform;
    protected float originPos;
    public virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if(isMove)
            originPos = rectTransform.anchoredPosition.x;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (clickClip != null)
            {
                AudioManager.PlayAudioRandPitch(clickClip);
            }
        });

        Kill();

        if (isMove)
            rectTransform.DOAnchorPosX(originPos, 0.5f).SetUpdate(true);

        rectTransform.DOScale(1, 0.25f).SetUpdate(true);
        foreach (var mat in textList)
        {
            mat.DOColor(originColor, 0.4f).SetUpdate(true);
        }
        foreach (var mat in imageList)
        {
            mat.DOColor(originColor, 0.2f).SetUpdate(true);
        }
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Kill();

        if (isMove)
            rectTransform.DOAnchorPosX(originPos + activeMoveValue, 0.25f).SetUpdate(true);
        
        rectTransform.DOScale(activeSizeValue, 0.2f).SetUpdate(true);
        foreach (var mat in textList)
        {
            mat.DOColor(activeColor, 0.2f).SetUpdate(true);
        }
        foreach (var mat in imageList)
        {
            mat.DOColor(activeColor, 0.2f).SetUpdate(true);
        }
        if (hoverClip != null)
        {
            AudioManager.PlayAudioRandPitch(hoverClip);
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Kill();

        if (isMove)
            rectTransform.DOAnchorPosX(originPos, 0.5f).SetUpdate(true);

        rectTransform.DOScale(1, 0.25f).SetUpdate(true);
        foreach (var mat in textList)
        {
            mat.DOColor(originColor, 0.4f).SetUpdate(true);
        }
        foreach (var mat in imageList)
        {
            mat.DOColor(originColor, 0.2f).SetUpdate(true);
        }
    }

    private void OnDisable()
    {
        Kill();

        if(isMove)
            rectTransform.anchoredPosition = new Vector2(originPos, rectTransform.anchoredPosition.y);
        
        rectTransform.localScale = Vector3.one;
        foreach (var mat in textList)
        {
            mat.color = originColor;
        }
    }

    protected virtual void Kill()
    {
        foreach (var mat in textList)
        {
            mat.DOKill();
        }
        foreach (var mat in imageList)
        {
            mat.DOKill();
        }
        rectTransform.DOKill();
    }
}
