using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private List<StageArea> stageList;

    [SerializeField] 
    private StageDataSO stageData;

    //[SerializeField]
    //private List<StageAreaDataSO> stageAreaList;

    public StageArea curStage;


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
        StartCoroutine(StageCycle());
    }

    IEnumerator StageCycle()
    {
        for (int i = 0; i < stageList.Count; i++)
        {
            stageList[i].EntryArea();
            yield return new WaitUntil(() => stageList[i].IsClear);
            stageList[i].IsClear = false;
        }
        EndManager.Instance.End();
    }
}
