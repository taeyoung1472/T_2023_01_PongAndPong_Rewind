using DG.Tweening;
using Shapes;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static Define;

public class Analysis : MonoBehaviour
{
    [SerializeField] private Disc background;
    [SerializeField] private Disc fill;
    [SerializeField] private Disc outer;
    [SerializeField] private Disc inner;
    [SerializeField] private Disc colorUI;
    [SerializeField] private CanvasGroup _buttonGroup = null;
    [SerializeField] private TextMeshProUGUI _collectingPercentText = null;
    [SerializeField] private TextMeshProUGUI _collcetObjectNameText = null;
    [SerializeField] private TextMeshProUGUI _worldNameText = null;

    [SerializeField]
    private Transform _collectObjParent = null;
    [SerializeField]
    private StageDatabase _worldDatabase = null;
    private WorldDataSO _curData = null;

    private int _maxCount = 8;
    private int _curCount = 0;
    private int _curIndex = 0;

    [SerializeField]
    private float _percentTextAnimationTime = 1f;

    private GameObject _curCollectObj = null;
    private Sequence _startingSeq = null;
    private Sequence _countUpSeq = null;
    private Coroutine _percentTextCoroutine = null;

    private bool isOuterRotating;

    private void OnEnable()
    {
        _curData = _worldDatabase.worldList[0];
        StartingAnimation();
        SetUp();
    }

    private void StartingAnimation()
    {
        if (_startingSeq != null)
            _startingSeq.Kill();

        isOuterRotating = false;
        background.AngRadiansEnd = 90 * Mathf.Deg2Rad;
        fill.AngRadiansEnd = 90 * Mathf.Deg2Rad;
        inner.AngRadiansEnd = 90 * Mathf.Deg2Rad;
        colorUI.Color = new Color(1, 1, 1, 0);
        outer.DashOffset = 0f;
        outer.AngRadiansEnd = 90 * Mathf.Deg2Rad;
        _buttonGroup.alpha = 0f;
        _buttonGroup.interactable = false;

        _startingSeq = DOTween.Sequence();
        _startingSeq.Append(DOTween.To(() => background.AngRadiansEnd, x => background.AngRadiansEnd = x, 450 * Mathf.Deg2Rad, 1f));
        _startingSeq.Append(DOTween.To(() => outer.AngRadiansEnd, x => outer.AngRadiansEnd = x, 450 * Mathf.Deg2Rad, 1f));
        _startingSeq.Join(DOTween.To(() => inner.AngRadiansEnd, x => inner.AngRadiansEnd = x, 450 * Mathf.Deg2Rad, 0.5f));
        _startingSeq.AppendCallback(() => isOuterRotating = true);
        _startingSeq.Append(DOTween.To(() => colorUI.Color, x => colorUI.Color = x, new Color(1, 0.4f, 0), 1f));
        _startingSeq.Join(_buttonGroup.DOFade(1f, 0.5f).OnComplete(() => _buttonGroup.interactable = true));
    }

    private void Update()
    {
        if (isOuterRotating)
        {
            outer.DashOffset += Time.deltaTime * 0.25f;
        }
    }

    public void RightMove()
    {
        _curIndex++;
        SetUp();
    }

    public void LeftMove()
    {
        _curIndex--;
        SetUp();
    }

    private void SetUp()
    {
        int prevIndex = _curIndex;
        _curIndex = Mathf.Clamp(_curIndex, 0, _worldDatabase.worldList.Count - 1);
        if (prevIndex != _curIndex)
            return;
        _curData = _worldDatabase.worldList[_curIndex];
        _maxCount = _curData.rewardCount;
        _curCount = player.playerJsonData.collectDatas[_curData.worldName];

        if (_curData.collectObject != null)
        {
            if (_curCollectObj != null)
                Destroy(_curCollectObj);
            _curCollectObj = Instantiate(_curData.collectObject, _collectObjParent);
        }
        _collcetObjectNameText.SetText(_curData.collectObjectName);
        _worldNameText.SetText("�������� " + _curData.worldName);
        if (_percentTextCoroutine != null)
            StopCoroutine(_percentTextCoroutine);
        _percentTextCoroutine = StartCoroutine(PercentTextCoroutine());
        UIAnimation();
    }

    private IEnumerator PercentTextCoroutine()
    {
        float tagetPercent = ((float)_curCount / _maxCount) * 100f;
        float time = 0f;
        while (time <= 1f)
        {
            _collectingPercentText.SetText($"�����\n{(tagetPercent * time).ToString("N0")}%");
            time += Time.deltaTime * (1 / _percentTextAnimationTime);
            yield return null;
        }
        _collectingPercentText.SetText($"�����\n{(tagetPercent).ToString("N0")}%");
    }

    private void UIAnimation()
    {
        fill.AngRadiansEnd = 90 * Mathf.Deg2Rad;

        if (_countUpSeq != null)
            _countUpSeq.Kill();
        _countUpSeq = DOTween.Sequence();
        _countUpSeq.Append(DOTween.To(() => fill.AngRadiansEnd, x => fill.AngRadiansEnd = x, (90f + (_curCount * (360f / _maxCount))) * Mathf.Deg2Rad, 1f));
    }
}
