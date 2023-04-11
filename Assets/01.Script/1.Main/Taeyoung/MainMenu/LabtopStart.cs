using DG.Tweening;
using Shapes;
using System.Collections;
using UnityEngine;

public class LabtopStart : MonoBehaviour
{
    [Header("Loading")]
    [SerializeField] private CanvasGroup loadingGroup;
    [SerializeField] private Disc loadingDisc;
    [SerializeField] private float loadingSpeed = 10;
    [SerializeField] private AnimationCurve loadingSpeedCurve;

    IEnumerator Start()
    {
        while (true)
        {   
            yield return null;
            loadingDisc.DashOffset += Time.deltaTime * loadingSpeed * loadingSpeedCurve.Evaluate(Time.time % 1);
            if (loadingDisc.DashOffset > 6)
                break;
        }
        //DOTween.To(x );
    }
}
