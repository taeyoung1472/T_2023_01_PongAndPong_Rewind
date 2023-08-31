using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeStampDataBase;

public class TimeStampManager : MonoSingleTon<TimeStampManager>
{
    #region DB
    private static TimeStampDataBase dataBase;
    private TimeStampDataBase DataBase
    {
        get
        {
            if (dataBase == null)
            {
                dataBase = ResourceManager.Load<TimeStampDataBase>("Core/Data/StampDB");
                dataBase.GenDic();
            }
            return dataBase;
        }
    }
    #endregion
    [SerializeField] private RectTransform timePivot;
    [SerializeField] private TimeStampDisplay displayPrefab;
    [SerializeField] private Transform displayParent;

    public void Awake()
    {
        RewindManager.Instance.InitPlay += ClearDisplay;
    }

    public void ClearDisplay()
    {
        for (int i = 0; i < displayParent.childCount; i++)
        {
            GameObject obj = displayParent.GetChild(i).gameObject;
            Destroy(obj);
        }
    }

    public void SetStamp(StampType stampType)
    {
        DisplayStamp(DataBase.GetData(stampType), DataBase.GetData(stampType).stampColor);
    }

    public void SetStamp(StampType stampType, Color backgroundColor)
    {
        DisplayStamp(DataBase.GetData(stampType), backgroundColor);
    }

    private void DisplayStamp(StampData data, Color backgroundColor)
    {
        Debug.Log("¿Ö ÀÛµ¿ ¾ÈÇÔ");
        if (RewindManager.Instance.IsBeingRewinded)
            return;
        Debug.Log("¾¾´í");

        TimeStampDisplay display = Instantiate(displayPrefab, displayParent);
        display.GetComponent<RectTransform>().position = GetStampPosition();
        display.Set(data.sprite, backgroundColor, TimerManager.Instance.CurrentTimer);
        display.gameObject.SetActive(true);
    }

    public Vector2 GetStampPosition()
    {
        return timePivot.position;
    }
}
