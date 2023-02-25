using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectUI : MonoBehaviour
{
    private ScrollRect _scrollRect = null;
    [SerializeField]
    private TextMeshProUGUI _worldNameText = null;
    private StageWorldUI _curStageWorld = null;
    private List<StageWorldUI> _stageWorlds = new List<StageWorldUI>();

    private StageUnitUI _curStage = null;
    private StageUnitUI _prevStage = null;

    private int _worldIndex = 0;

    private Sequence _worldUIMoveSeq = null;

    [SerializeField]
    private float _sizeChangeDuration = 0.1f;
    [SerializeField]
    private Color _accentColor = Color.white;
    [SerializeField]
    private Color _deAccentColor = Color.white;
    [SerializeField]
    private float _sizeUpAmount = 1.3f;
    [SerializeField]
    private float _sizeDownAmount = 0.8f;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _stageWorlds.AddRange(GetComponentsInChildren<StageWorldUI>());
        _curStageWorld = _stageWorlds[0];
        for (int i = 1; i < _stageWorlds.Count; i++)
            _stageWorlds[i].gameObject.SetActive(false);
        WorldChange(0);
    }

    public void WorldUp()
    {
        WorldChange(_worldIndex + 1);
    }

    public void WorldDown()
    {
        WorldChange(_worldIndex - 1);
    }

    public void WorldChange(int value)
    {
        if (_worldUIMoveSeq != null)
            _worldUIMoveSeq.Kill();
        _curStageWorld.ResetWorld();
        _curStageWorld.gameObject.SetActive(false);
        _scrollRect.content.anchoredPosition = Vector3.zero;
        _curStage = _prevStage = null;
        _worldIndex = value;
        _worldIndex = Mathf.Clamp(_worldIndex, 0, _stageWorlds.Count - 1);
        _curStageWorld = _stageWorlds[_worldIndex];
        _curStageWorld.gameObject.SetActive(true);
        _scrollRect.content = _curStageWorld.GetComponent<RectTransform>();
        _worldNameText.SetText(_curStageWorld.WorldName);
    }

    public void StageChange()
    {
        if (_curStage == null)
        {
            Debug.Log("º±≈√ æ» µ ");
        }
        DOTween.KillAll();
        LoadingSceneManager.LoadScene(2);
    }

    private void Update()
    {
        if(_curStageWorld != null)
        {
            if (_curStageWorld.gameObject.activeSelf == false)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                if (_worldUIMoveSeq != null)
                    _worldUIMoveSeq.Kill();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _prevStage = _curStage;
                _curStage = _curStageWorld.MouseUp(_deAccentColor, _sizeDownAmount, _sizeChangeDuration, _accentColor, _sizeUpAmount);
                if (_worldUIMoveSeq != null)
                    _worldUIMoveSeq.Kill();
                _worldUIMoveSeq = DOTween.Sequence();
                float target = _curStage.GetComponent<RectTransform>().anchoredPosition.x * -1f;
                _worldUIMoveSeq.Append(_curStageWorld.GetComponent<RectTransform>().DOAnchorPosX(target, _sizeChangeDuration));
            }
        }
    }
}
