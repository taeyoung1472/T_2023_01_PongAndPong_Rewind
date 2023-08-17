using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class StageSelectUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _chapterSelectUI = null;
    [SerializeField]
    private GameObject _worldUI = null;

    [SerializeField]
    private StageInfoUI _stageInfoUI = null;

    [SerializeField]
    private ScrollRect _worldScrollRect = null;
    [SerializeField]
    private ScrollRect _chapterScrollRect = null;
    [SerializeField]
    private TextMeshProUGUI _worldNameText = null;
    [SerializeField]
    private TextMeshProUGUI _worldChapterNameText = null;
    private StageWorldUI _curStageWorld = null;
    public StageWorldUI CurStageWorld => _curStageWorld;

    [SerializeField]
    private Transform _worldTrm = null;
    [SerializeField]
    private Transform _chapterTrm = null;
    [SerializeField]
    private GameObject _chapterButtonPrefab = null;

    private List<StageWorldUI> _stageWorlds = new List<StageWorldUI>();
    private List<GameObject> _chapterButtons = new List<GameObject>();

    private StageUnitUI _curStage = null;
    private StageUnitUI _prevStage = null;

    private int _worldIndex = 0;
    public int WorldIndex => _worldIndex;
    private bool _lock = false;
    public bool Lock { get => _lock; set
        {
            _lock = value;
            //_worldScrollRect.enabled = !value;
        }
    }
    private bool _moveLock = false;

    private Sequence _worldUIMoveSeq = null;

    [SerializeField]
    private float _sizeChangeDuration = 0.1f;
    [SerializeField]
    private Color _accentColor = Color.white;
    [SerializeField]
    private Color _subAccentColor = Color.white;
    [SerializeField]
    private Color _deAccentColor = Color.white;
    [SerializeField]
    private float _sizeUpAmount = 1.3f;
    [SerializeField]
    private float _sizeSubAmount = 1.2f;
    [SerializeField]
    private float _sizeDownAmount = 0.8f;

    public void Init(List<GameObject> worlds)
    {
        _worldIndex = 0;
        for (int i = 0; i < _stageWorlds.Count; i++)
        {
            Destroy(_stageWorlds[i].gameObject);
            Destroy(_chapterButtons[i].gameObject);
        }
        _stageWorlds.Clear();
        _chapterButtons.Clear();

        GridLayoutGroup gridGroup = _chapterScrollRect.content.GetComponent<GridLayoutGroup>();
        float padding = gridGroup.padding.left + gridGroup.spacing.x;
        float width = padding * 2 + gridGroup.cellSize.x * worlds.Count;
        Debug.Log(width);
        _chapterScrollRect.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

        List<StageWorldUI> newWorlds = new List<StageWorldUI>();
        for (int i = 0; i < worlds.Count; i++)
        {
            StageWorldUI ui = Instantiate(worlds[i], _worldTrm).GetComponent<StageWorldUI>();
            newWorlds.Add(ui);
            ui.Init(this);
            ui.gameObject.SetActive(false);
            Button chapter = Instantiate(_chapterButtonPrefab, _chapterTrm).GetComponent<Button>();
            chapter.onClick.AddListener(() => { WorldChange(ui); });
            chapter.GetComponent<ChapterButtonUI>().NameSet(ui);
            _chapterButtons.Add(chapter.gameObject);
        }
        _stageWorlds = newWorlds;
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
        _moveLock = false;
        if (_curStageWorld != null)
        {
            _curStageWorld.ResetWorld();
            _curStageWorld.gameObject.SetActive(false);
        }
        if (_worldScrollRect.content != null)
            _worldScrollRect.content.anchoredPosition = Vector3.zero;
        _curStage = _prevStage = null;

        _worldIndex = value;
        _worldIndex = Mathf.Clamp(_worldIndex, 0, _stageWorlds.Count - 1);
        _curStageWorld = _stageWorlds[_worldIndex];
        _curStageWorld.gameObject.SetActive(true);
        _worldScrollRect.content = _curStageWorld.GetComponent<RectTransform>();
        _worldNameText.SetText(_curStageWorld.WorldName);
        _worldChapterNameText.SetText("√©≈Õ " + _curStageWorld.ChapterName);

        _curStage = _curStageWorld.GetStage(0);
        float target = _curStage.GetComponent<RectTransform>().anchoredPosition.x * -1f;
        RectTransform trm = _curStageWorld.GetComponent<RectTransform>();
        Vector2 anPos = trm.anchoredPosition;
        anPos.x = target;
        trm.anchoredPosition = anPos;

        _worldUI.SetActive(true);
        _chapterSelectUI.SetActive(false);
        _worldNameText.gameObject.SetActive(true);
        _worldChapterNameText.gameObject.SetActive(true);
    }

    public void WorldChange(StageWorldUI ui)
    {
        int index = _stageWorlds.FindIndex(x => x == ui);
        Debug.Log(index);
        WorldChange(index);
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

    public void UIOn()
    {
        _worldUI.SetActive(false);
        _chapterSelectUI.SetActive(true);
        _worldNameText.gameObject.SetActive(false);
        _worldChapterNameText.gameObject.SetActive(false);
        gameObject.SetActive(true);
        _curStage = _prevStage = null;
        _moveLock = false;
        _curStageWorld = null;
    }

    public void UIDown()
    {
        if (_chapterSelectUI.activeSelf)
        {
            player.PlayerActionExit(PlayerActionType.Interact);
            if (_curStageWorld != null)
            {
                _curStageWorld.ResetWorld();
                _curStageWorld.gameObject.SetActive(false);
            }
            _curStage = _prevStage = null;
            _curStageWorld = null;
            gameObject.SetActive(false);
        }
        else if (_worldUI.activeSelf)
        {
            _worldUI.SetActive(false);
            _chapterSelectUI.SetActive(true);
            _worldNameText.gameObject.SetActive(false);
            _worldChapterNameText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_lock) return;
            UIDown();
            return;
        }

        if (_moveLock) return;
        if (_curStageWorld != null)
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
                    WorldUISet();
            }
        }
    }

    private void WorldUISet(StageUnitUI ui = null)
    {
        _moveLock = true;
        _prevStage = _curStage;
        _curStage = _curStageWorld.MouseUp(_accentColor, _subAccentColor, _deAccentColor, _sizeUpAmount, _sizeSubAmount, _sizeDownAmount, _sizeChangeDuration, ui);
        if (_stageInfoUI.state == StageInfoUIState.On && _prevStage != _curStage)
            _stageInfoUI.UIDown();
        if (_worldUIMoveSeq != null)
            _worldUIMoveSeq.Kill();
        _worldUIMoveSeq = DOTween.Sequence();
        float target = _curStage.GetComponent<RectTransform>().anchoredPosition.x * -1f;
        _worldUIMoveSeq.Append(_curStageWorld.GetComponent<RectTransform>().DOAnchorPosX(target, _sizeChangeDuration));
        _worldUIMoveSeq.AppendCallback(() => { _moveLock = false; });
    }

    public void StageSelect(StageUnitUI ui)
    {
        if (_curStage == ui)
        {
            _stageInfoUI.UIOn(ui.StageDataSO);
        }
        else
        {
            if (_stageInfoUI.state == StageInfoUIState.Animation)
                return;
            WorldUISet(ui);
        }
    }
    public void RoadGameScene()
    {
        _curStage.SetStageData();
        LoadingSceneManager.LoadScene(1);
    }
}
