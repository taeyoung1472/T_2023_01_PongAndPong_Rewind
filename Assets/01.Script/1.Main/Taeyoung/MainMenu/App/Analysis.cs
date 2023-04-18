using DG.Tweening;
using Shapes;
using UnityEngine;

public class Analysis : MonoBehaviour
{
    [SerializeField] private Disc background;
    [SerializeField] private Disc fill;
    [SerializeField] private Disc outer;
    [SerializeField] private Disc inner;
    [SerializeField] private Disc ¸àÁê¹Þ¾Ò¾î;

    [SerializeField]
    private WorldDataSO _curData = null;

    private int _maxCount = 8;
    private int _curCount = 0;

    private Sequence _countUpSeq = null;

    private bool isOuterRotating;

    private void OnEnable()
    {
        SetUp();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Up();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            CountDown();
        }

        if (isOuterRotating)
        {
            outer.DashOffset += Time.deltaTime * 0.25f;
        }
    }

    private void SetUp()
    {
        _maxCount = _curData.rewardCount;

        isOuterRotating = false;
        background.AngRadiansEnd = 90 * Mathf.Deg2Rad;
        fill.AngRadiansEnd = 90 * Mathf.Deg2Rad;
        inner.AngRadiansEnd = 90 * Mathf.Deg2Rad;
        ¸àÁê¹Þ¾Ò¾î.Color = new Color(1, 1, 1, 0);
        //outer.DashSpacing = 1;
        outer.DashOffset = 0f;
        outer.AngRadiansEnd = 90 * Mathf.Deg2Rad;

        Sequence seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => background.AngRadiansEnd, x => background.AngRadiansEnd = x, 450 * Mathf.Deg2Rad, 1f));
        seq.Append(DOTween.To(() => outer.AngRadiansEnd, x => outer.AngRadiansEnd = x, 450 * Mathf.Deg2Rad, 1f));
        seq.Join(DOTween.To(() => inner.AngRadiansEnd, x => inner.AngRadiansEnd = x, 450 * Mathf.Deg2Rad, 0.5f));
        //seq.Append(DOTween.To(() => outer.DashSpacing, x => outer.DashSpacing = x, 0.3f, 1f));
        seq.AppendCallback(() => isOuterRotating = true);
        seq.Append(DOTween.To(() => ¸àÁê¹Þ¾Ò¾î.Color, x => ¸àÁê¹Þ¾Ò¾î.Color = x, new Color(1, 0.4f, 0), 1f));
    }

    public void Up()
    {
        _curCount++;
        _curCount = Mathf.Clamp(_curCount, 0, _maxCount);
        FunctionManager.Instance.GetEvent(_curData.GetFunctionName(_curCount))?.Invoke();

        if (_countUpSeq != null)
            _countUpSeq.Kill();
        _countUpSeq = DOTween.Sequence();
        _countUpSeq.Append(DOTween.To(() => fill.AngRadiansEnd, x => fill.AngRadiansEnd = x, (90f + (_curCount * (360f / _maxCount))) * Mathf.Deg2Rad, 1f));
    }

    public void CountDown()
    {
        _curCount--;
        _curCount = Mathf.Clamp(_curCount, 0, _maxCount);
        FunctionManager.Instance.GetEvent(_curData.GetFunctionName(_curCount))?.Invoke();
        if (_countUpSeq != null)
            _countUpSeq.Kill();
        _countUpSeq = DOTween.Sequence();
        _countUpSeq.Append(DOTween.To(() => fill.AngRadiansEnd, x => fill.AngRadiansEnd = x, (90f + (_curCount * (360f / _maxCount))) * Mathf.Deg2Rad, 1f));
    }
}
