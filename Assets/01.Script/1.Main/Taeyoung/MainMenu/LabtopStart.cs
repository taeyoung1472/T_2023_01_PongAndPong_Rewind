using DG.Tweening;
using Shapes;
using System.Collections;
using UnityEngine;

public class LabtopStart : MonoBehaviour
{
    [Header("Loading")]
    [SerializeField] private Disc loadingDisc;
    [SerializeField] private float loadingSpeed = 10;
    [SerializeField] private AnimationCurve loadingSpeedCurve;

    IEnumerator Start()
    {
        while (true)
        {   
            yield return null;
            loadingDisc.DashOffset += Time.deltaTime * loadingSpeed * loadingSpeedCurve.Evaluate(Time.time % 1);
            if (loadingDisc.DashOffset > 4)
            {
                DOTween.To(() => loadingDisc.Color, x => loadingDisc.Color = x, new Color(1, 1, 1, 0), 1f);
                break;
            }
        }
        yield return new WaitForSeconds(1.5f);
    }
}
