using DG.Tweening;
using Shapes;
using UnityEngine;

public class Analysis : MonoBehaviour
{
    [SerializeField] private Disc background;
    [SerializeField] private Disc fill;
    [SerializeField] private Disc outer;
    [SerializeField] private Disc inner;
    [SerializeField] private Disc ธเม๊นÞพาพ๎;

    private bool isOuterRotating;

    private void OnEnable()
    {
        SetUp();
    }

    private void Update()
    {
        if (isOuterRotating)
        {
            outer.DashOffset += Time.deltaTime * 0.25f;
        }
    }

    private void SetUp()
    {
        isOuterRotating = false;
        background.AngRadiansEnd = 90 * Mathf.Deg2Rad;
        fill.AngRadiansEnd = 90 * Mathf.Deg2Rad;
        inner.AngRadiansEnd = 90 * Mathf.Deg2Rad;
        ธเม๊นÞพาพ๎.Color = new Color(1, 1, 1, 0);
        //outer.DashSpacing = 1;
        outer.DashOffset = 0f;
        outer.AngRadiansEnd = 90 * Mathf.Deg2Rad;

        Sequence seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => background.AngRadiansEnd, x => background.AngRadiansEnd = x, 450 * Mathf.Deg2Rad, 1f));
        seq.Insert(0.5f, DOTween.To(() => fill.AngRadiansEnd, x => fill.AngRadiansEnd = x, (90f + 360f * Random.value) * Mathf.Deg2Rad, 1f));
        seq.Append(DOTween.To(() => outer.AngRadiansEnd, x => outer.AngRadiansEnd = x, 450 * Mathf.Deg2Rad, 1f));
        seq.Join(DOTween.To(() => inner.AngRadiansEnd, x => inner.AngRadiansEnd = x, 450 * Mathf.Deg2Rad, 0.5f));
        //seq.Append(DOTween.To(() => outer.DashSpacing, x => outer.DashSpacing = x, 0.3f, 1f));
        seq.AppendCallback(() => isOuterRotating = true);
        seq.Append(DOTween.To(() => ธเม๊นÞพาพ๎.Color, x => ธเม๊นÞพาพ๎.Color = x, new Color(1, 0.4f, 0), 1f));
    }
}
