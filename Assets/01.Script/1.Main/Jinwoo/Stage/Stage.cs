using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    [SerializeField] private List<StageArea> stageAreaList;

    [SerializeField] private StageDataSO stageData;

    public StageArea curArea;

    public void Init()
    {
        foreach (var area in stageAreaList)
        {
            area.isAreaClear = false;
        }


        StartCoroutine(StageCycle());
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            ReStartArea();
        }

    }
    private void ReStartArea()
    {
        if (!UIManager.Instance.IsPause)
        {
            if (TimerManager.Instance.isRewinding)
            {
                TimerManager.Instance.EndRewind();
            }
            else
            {
                StageManager.Instance.InitPlayer(false);
                curArea.EntryArea();
            }
        }
    }

    IEnumerator StageCycle()
    {
        for (int i = 0; i < stageAreaList.Count; i++)
        {
            curArea = stageAreaList[i];
            stageAreaList[i].EntryArea();
            yield return new WaitUntil(() => stageAreaList[i].isAreaClear);
            TimerManager.Instance.ChangeOnTimer(false);

            StageManager.Instance.fadeImg.gameObject.SetActive(true);

            yield return new WaitForSeconds(3f);

            StageManager.Instance.fadeImg.gameObject.SetActive(false);
            TimerManager.Instance.EndRewind();


        }
        EndManager.Instance.End();
    }
}
