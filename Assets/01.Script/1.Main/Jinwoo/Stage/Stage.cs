using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    [SerializeField] private List<StageArea> stageAreaList;

    [SerializeField]
    private StageDataSO stageData;

    public StageArea curArea;

    public void Init()
    {
        stageAreaList.AddRange(stageData.StageAreaList);

        foreach (var area in stageAreaList)
        {
            area.AreaData.isAreaClear = false;
        }

        //for (int i = int.MinValue; i <= int.MaxValue; i++)
        //{
        //    Debug.Log($"¾Ó±â¸ð¶ì{i}");
        //    Debug.Log($"{i}¶ì¸ð±â¾Ó");
        //}

        StartCoroutine(StageCycle());
    }

    IEnumerator StageCycle()
    {
        for (int i = 0; i < stageAreaList.Count; i++)
        {
            curArea = stageAreaList[i];
            stageAreaList[i].EntryArea();
            yield return new WaitUntil(() => stageAreaList[i].AreaData.isAreaClear);
            TimerManager.Instance.ChangeOnTimer(false);

            StageManager.Instance.fadeImg.gameObject.SetActive(true);

            yield return new WaitForSeconds(3f);

            StageManager.Instance.fadeImg.gameObject.SetActive(false);
            TimerManager.Instance.EndRewind();


        }
        EndManager.Instance.End();
    }
}
