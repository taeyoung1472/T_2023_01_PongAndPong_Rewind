using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class StageWorldUI : MonoBehaviour
{
    private readonly float BOX_WIDTH = 960f;
    private RectTransform _stageParentTrm = null;
    int clearCount = 1;

    private StageSelectUI _stageSelectUI = null;

    public RectTransform viewPort = null;

    public Sprite chapterBackgroundSprite;

    [SerializeField] private List<StageUnitUI> _stages = new List<StageUnitUI>();
    private List<RectTransform> _stageTrms = new List<RectTransform>();
    private RectTransform _thisTrm = null;
    private UILineRenderer _uiLineRenderer = null;

    public StageDatabase db;

    [SerializeField]
    private WorldType _worldType;
    public WorldType WorldType => _worldType;

    [SerializeField]
    private string _worldName;
    [SerializeField]
    private string _chapterName;

    public string WorldName => _worldName;
    public string ChapterName => _chapterName;

    public void UISet()
    {
        if (_stageSelectUI == null)
            return;
        _stageSelectUI.WorldChange(this);
    }

    public void Init(StageSelectUI ui)
    {
        _stageSelectUI = ui;
        _stages.AddRange(GetComponentsInChildren<StageUnitUI>());
        _stageParentTrm = transform.Find("StageParent").GetComponent<RectTransform>();

        for (int i = 0; i < _stages.Count; i++)
            Lis(_stageSelectUI, i);
        TrmSet();

        SetMapeActive();
        BoxSizeSetting();
        LineSet();
    }

    private void LineSet()
    {
        _uiLineRenderer = GetComponent<UILineRenderer>();
        var anchored = from v in _stageTrms
                       select v.anchoredPosition + _stageParentTrm.anchoredPosition;

        List<Vector2> pointList = anchored.ToList();
        int deleteCnt = _stages.Count - clearCount; //4 9
        if (clearCount <= 0)
        {
            pointList.RemoveRange(clearCount, deleteCnt); //2부터 6개삭제
        }

        Vector2[] pointArray = pointList.ToArray();

        _uiLineRenderer.Points = pointArray;
    }

    //뷰포트 크기 조정
    private void BoxSizeSetting()
    {
        Vector2 an = _stageParentTrm.anchoredPosition;
        an.x = BOX_WIDTH * 0.5f;
        _stageParentTrm.anchoredPosition = an;

        RectTransform lastTrm = _stageTrms[0];
        for (int i = 0; i < _stageTrms.Count; i++)
        {
            if (_stageTrms[i].gameObject.activeSelf == false)
            {
                break;
            }
            lastTrm = _stageTrms[i];
        }
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, BOX_WIDTH + lastTrm.anchoredPosition.x);
    }

    /// <summary>
    /// UI에서 동그라미 엑티브 세팅
    /// </summary>
    private void SetMapeActive()
    {
        SaveDataManager.Instance.LoadStageClearJSON();
        SaveDataManager.Instance.LoadCollectionJSON();

        for (int i = 0; i < SaveDataManager.Instance.AllChapterClearDataBase.stageClearDataDic[_worldName].stageClearDataList.Count; i++)
        {
            if (SaveDataManager.Instance.AllChapterClearDataBase.stageClearDataDic[_worldName].stageClearDataList[i].stageClearBoolData)
            {
                clearCount++;
            }
        }


        for (int i = 0; i < _stageTrms.Count; i++)
        {
            if (clearCount == 0)
            {
                return;
            }

            _stageTrms[i].gameObject.SetActive(true);
        }
    }

    private void TrmSet()
    {
        if (_thisTrm != null)
            return;

        _thisTrm = GetComponent<RectTransform>();
        for (int i = 0; i < _stages.Count; i++)
            _stageTrms.Add(_stages[i].GetComponent<RectTransform>());
    }

    private void Lis(StageSelectUI ui, int index)
    {
        _stages[index].GetComponentInChildren<Button>().onClick.AddListener(() => ui.StageSelect(_stages[index]));
    }

    public StageUnitUI GetStage(int index)
    {
        index = Mathf.Clamp(index, 0, _stages.Count - 1);
        return _stages[index];
    }

    public void ResetWorld()
    {
        for (int i = 0; i < _stages.Count; i++)
        {
            _stages[i].AccectReset();
        }
    }

    public StageUnitUI MouseUp(Color accentColor, Color subAccentColor, Color deAccentColor, float sizeUpAmount, float sizeSubAmount, float sizeDownAmount, float sizeChangeDuration, StageUnitUI ui = null)
    {
        int minIndex = 0;

        if (ui == null)
        {
            float minX = Mathf.Abs(_stageTrms[0].anchoredPosition.x + _thisTrm.anchoredPosition.x);
            for (int i = 1; i < _stages.Count; i++)
            {
                if (_stageTrms[i].gameObject.activeSelf == false)
                {
                    break;
                }

                float curX = Mathf.Abs(_stageTrms[i].anchoredPosition.x + _thisTrm.anchoredPosition.x);
                if (minX > curX)
                {
                    minX = curX;
                    minIndex = i;
                }
            }
        }
        else
        {
            for (int i = 0; i < _stages.Count; i++)
            {
                if (ui == _stages[i])
                {
                    minIndex = i;
                    break;
                }
            }
        }

        for (int i = 0; i < _stages.Count; i++)
            _stages[i].UIAccent(deAccentColor, sizeDownAmount, sizeChangeDuration);
        _stages[GetIndex(minIndex - 1)].UIAccent(subAccentColor, sizeSubAmount, sizeChangeDuration);
        _stages[GetIndex(minIndex + 1)].UIAccent(subAccentColor, sizeSubAmount, sizeChangeDuration);
        _stages[minIndex].UIAccent(accentColor, sizeUpAmount, sizeChangeDuration, true);
        return _stages[minIndex];
    }

    private int GetIndex(int value)
    {
        value = Mathf.Clamp(value, 0, _stages.Count - 1);
        return value;
    }
}
