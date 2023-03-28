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

    public void ReStartArea()
    {
        if (!UIManager.Instance.IsPause)
        {
            //여기에 코루틴 넣은뒤 먼가 다시하기 연출 살짝 넣으면 좋을 지도...(추후 예정임)
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
