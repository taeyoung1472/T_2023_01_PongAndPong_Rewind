using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class FadeInOutManager : MonoSingleTon<FadeInOutManager>
{
    [SerializeField] private Image fadeImg;

    private void Start()
    {
        FadeOut(6f);
    }

    public void FadeIn(float duration)
    {
        fadeImg.gameObject.SetActive(true);
        fadeImg.DOFade(1, duration);
    }
    public void FadeOut(float duration)
    {
        fadeImg.gameObject.SetActive(true);
        fadeImg.DOFade(0, duration);
    }
}
