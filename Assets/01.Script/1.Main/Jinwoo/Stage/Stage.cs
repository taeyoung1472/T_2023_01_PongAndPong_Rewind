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

    public void ReStartArea(bool isReStart)
    {
        StartCoroutine(StartReStart(isReStart));

    }
    public IEnumerator StartReStart(bool isReStart)
    {
        if (isReStart) //만약 리스타트면 연출 넣고 
        {
            BreakScreenController.Instance.StartBreakScreen();
        }
        else //그냥 자유시점이면 연출 다른거나 안넣고
        {
            
        }
        yield return new WaitUntil(()=> !BreakScreenController.Instance.isBreaking);

        yield return new WaitForSeconds(1f);



        if (TimerManager.Instance.isRewinding) //만약 리와인드 상태에서 재시작할때
        {
            TimerManager.Instance.EndRewind();
        }
        else // 일반 순행 시간에 재시작 할때
        {
            Debug.Log("리스타또");
            StageManager.Instance.InitPlayer(false);
            transform.DOKill();
            transform.rotation = Quaternion.Euler(0, 0, 0);
            curArea.EntryArea(true);
        }

        RewindManager.Instance.RestartPlay?.Invoke();
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
