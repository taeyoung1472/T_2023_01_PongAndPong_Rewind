using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleTon<StageManager>
{
    [SerializeField]
    private StageDataSO stageData;

    public StageAreaT curArea;

    [SerializeField] private List<StageAreaT> stageAreaList;

    public int stageNum = 0;
    public bool stageClear = false;

    public void Init()
    {
        stageNum = 0;
        //curStage = Instantiate(stageData.stagePrefab, Vector3.zero, Quaternion.identity);
        InitSetData();
    }

    public void InitSetData()
    {
        stageAreaList.AddRange(stageData.StageAreaList);

        foreach (var area in stageAreaList)
        {
            area.IsClear = false;
        }
        //curArea = stageAreaList[stageNum];

        StartCoroutine(StageCycle());
    }
    public void SetArea(StageAreaT area)
    {
        if (curArea != null)
            curArea.ExitArea();

        if (curArea == area)
        {
            area.EntryArea(true);
        }
        curArea = area;
    }

    IEnumerator StageCycle()
    {
        for (int i = 0; i < stageAreaList.Count; i++)
        {
            //curArea = stageAreaList[i];
            SetArea(stageAreaList[i]);
            //ReTimeManager.Instance.Init();
            curArea.EntryArea();
            yield return new WaitUntil(() => curArea.IsClear); //클리어 했을 경우에만 다음으로 넘어감
            stageNum++;
            Debug.Log("대음");
            curArea.IsClear = false;
            curArea.IsRewind = false;
            ReTimeManager.Instance.Init();
            curArea.ExitArea();
        }
        stageClear = true;
        EndManager.Instance.End();
    }
}
