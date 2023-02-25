using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class StageWorldUI : MonoBehaviour
{
    private List<StageUnitUI> _stages = new List<StageUnitUI>();
    [SerializeField]
    private string _worldName = "";
    public string WorldName => _worldName;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _stages.AddRange(GetComponentsInChildren<StageUnitUI>());
    }

    public void ResetWorld()
    {
        for(int i = 0; i < _stages.Count; i++)
        {
            _stages[i].AccectReset();
        }
    }

    public StageUnitUI MouseUp(Color deAccentColor, float sizeDownAmount, float sizeChangeDuration, Color accentColor, float sizeUpAmount)
    {
        int minIndex = 0;
        float minX = Mathf.Abs((Screen.currentResolution.width / 2) - Mathf.Abs(_stages[0].transform.position.x));
        for (int i = 1; i < _stages.Count; i++)
        {
            float curX = Mathf.Abs((Screen.currentResolution.width / 2) - Mathf.Abs(_stages[i].transform.position.x));
            if (minX > curX)
            {
                minX = curX;
                minIndex = i;
            }
        }
        StageUnitUI targetUI = _stages[minIndex];
        for (int i = 0; i < _stages.Count; i++)
            _stages[i].UIAccent(deAccentColor, sizeDownAmount, sizeChangeDuration);
        _stages[GetIndex(minIndex - 1)].UIAccent(Color.white, 1f, sizeChangeDuration);
        _stages[GetIndex(minIndex + 1)].UIAccent(Color.white, 1f, sizeChangeDuration);
        targetUI.UIAccent(accentColor, sizeUpAmount, sizeChangeDuration);
        return targetUI;
    }

    private int GetIndex(int value)
    {
        value = Mathf.Clamp(value, 0, _stages.Count - 1);
        return value;
    }
}
