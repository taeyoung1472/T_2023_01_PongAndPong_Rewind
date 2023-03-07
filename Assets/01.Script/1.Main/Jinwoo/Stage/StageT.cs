using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageT : MonoBehaviour
{
    [SerializeField] private List<StageAreaT> stageAreaList;

    [SerializeField]
    private StageDataSO stageData;

    //[SerializeField]
    //private List<StageAreaDataSO> stageAreaList;

    public StageAreaT curArea;

    private void Awake()
    {
        //if (stageAreaList.Count <= 0)
        //{
        //    stageAreaList.AddRange(stageData.StageAreaPrefab);
        //}

    }
    public void Start()
    {
        //RewindManager.Instance.Init();
        
    }

    public void InitSetData(StageDataSO dataSO)
    {
        stageData = dataSO;
        stageAreaList.AddRange(stageData.StageAreaList);

        foreach (var area in stageAreaList)
        {
            area.IsClear = false;
        }

        StartCoroutine(StageCycle());
    }

    IEnumerator StageCycle()
    {
        for (int i = 0; i < stageAreaList.Count; i++)
        {
            curArea = stageAreaList[i];
            stageAreaList[i].EntryArea();
            yield return new WaitUntil(() => stageAreaList[i].IsClear);
        }
        EndManager.Instance.End();
    }
}
