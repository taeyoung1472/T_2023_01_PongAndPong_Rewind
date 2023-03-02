using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private List<StageArea> stageList;

    private bool isClearNext = false;
    public bool IsClearNext { set { isClearNext = value; } }

    [SerializeField] 
    private StageDataSO stageData;

    [SerializeField]
    private List<StageAreaDataSO> stageAreaList;
    private void Awake()
    {
        stageAreaList.AddRange(stageData.StageAreaPrefab);
        
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
            yield return new WaitUntil(() => isClearNext);
            isClearNext = false;
        }
        EndManager.Instance.End();
    }
}
