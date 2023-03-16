using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTestManager : MonoSingleTon<StageTestManager>
{
    [SerializeField]
    private StageDataSO stageData;

    public StageAreaT curArea;

    [SerializeField] private List<StageAreaT> stageAreaList;

    public int stageAreaNum = 0;
    public bool curstageClear = false;

    public bool isStageAreaPlayStart = false;
    private void Awake()
    {
        Init();
    }
    public void Init()//ù ���������� ������ ����
    {
        isStageAreaPlayStart = false;
        stageAreaNum = 0;
        //curStage = Instantiate(stageData.stagePrefab, Vector3.zero, Quaternion.identity);
        InitSetData();
    }

    public void InitSetData() //ù ���������� ������ ����
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
        {
            area.ExitArea();
        }
        if (curArea == area)
        {
            area.EntryArea(true);
        }
        else
            area.EntryArea();

        curArea = area;
    }

    IEnumerator StageCycle()
    {
        for (int i = 0; i < stageAreaList.Count; i++)
        {
            SetArea(stageAreaList[i]);
            isStageAreaPlayStart = true;
            yield return new WaitUntil(() => curArea.IsClear); //Ŭ���� ���� ��쿡�� �������� �Ѿ
            isStageAreaPlayStart = false;
            stageAreaNum++;
            Debug.Log("����");
            curArea.IsClear = false;
            //ReTimeManager.Instance.Init();
            curArea.ExitArea();
        }
        isStageAreaPlayStart = false;
        curstageClear = true;
        EndManager.Instance.End();
    }

}
