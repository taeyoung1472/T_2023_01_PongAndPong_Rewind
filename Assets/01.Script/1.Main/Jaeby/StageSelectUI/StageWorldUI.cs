using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Linq;

public class StageWorldUI : MonoBehaviour
{
    private StageSelectUI _stageSelectUI = null;

    public RectTransform viewPort = null;

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

       


        for (int i = 0; i < _stages.Count; i++)
            Lis(_stageSelectUI, i);
        TrmSet();



        SetMapeActive();
    }
  
    /// <summary>
    /// UI에서 동그라미 엑티브 세팅
    /// </summary>
    private void SetMapeActive()
    {
        SaveDataManager.Instance.LoadStageClearJSON();

        int clearCount = 0;

        for (int i = 0; i < SaveDataManager.Instance.AllChapterClearDataBase.stageClearDataDic[_worldName].stageClearDataList.Count; i++)
        {
            if (SaveDataManager.Instance.AllChapterClearDataBase.stageClearDataDic[_worldName].stageClearDataList[i].stageClearBoolData)
            {
                clearCount++;
            }
        }

        int activeCount = 0;

        if (clearCount == 0)
        {
            activeCount = 1;
            for (int i = 0; i < activeCount; i++)
            {
                _stages[i].gameObject.SetActive(true);
            }



            Debug.Log(clearCount + "클리어카운트가 0임");
        }
        else
        {
            Debug.Log(clearCount + "클리어카운트가 알잘딱");
            activeCount = clearCount + 1;
            for (int i = 0; i < activeCount; i++)
            {
                _stages[i].gameObject.SetActive(true);
            }
        }

        for (int i = activeCount; i < _stages.Count; i++)
        {
            _stages[i].gameObject.SetActive(false);
        }

        #region UI에서 선 부분

        _uiLineRenderer = GetComponent<UILineRenderer>();
        var anchored = from v in _stageTrms
                       select v.anchoredPosition;

        List<Vector2> pointList = anchored.ToList();
        int deleteCnt = _stages.Count - activeCount;
        pointList.RemoveRange(activeCount, deleteCnt); //2부터 6개삭제

        Vector2[] pointArray = pointList.ToArray();

        _uiLineRenderer.Points = pointArray;
        #endregion


        #region 뷰포트 크기 설정

        viewPort = _uiLineRenderer.GetComponent<RectTransform>();
        if (viewPort)
        {
            viewPort.sizeDelta = new Vector2(280 * activeCount, 300);
        }
        #endregion


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
