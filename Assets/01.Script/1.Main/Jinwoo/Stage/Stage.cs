using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stage : MonoBehaviour
{
    [SerializeField] private List<StageArea> stageAreaList;

    [SerializeField] private StageDataSO stageData;

    [HideInInspector] public StageArea curArea;

    public UnityEvent stageEndEvent;

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
        yield return new WaitUntil(() => !BreakScreenController.Instance.isBreaking);

        yield return new WaitForSeconds(.5f);



        if (TimerManager.Instance.isRewinding) //만약 리와인드 상태에서 재시작할때
        {
            transform.DOKill();
            transform.rotation = Quaternion.Euler(0, 0, 0);
            TimerManager.Instance.EndRewind();
        }
        else // 일반 순행 시간에 재시작 할때
        {
            //Debug.Log("리스타또");
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
            StageManager.Instance.InputLock = false;
            curArea = stageAreaList[i];
            StageManager.Instance.SetFreeLookCamInitPos(curArea.freeLookCamPos);
            if (i == 0)
                stageAreaList[i].EntryArea();
            else
            {
                //stageAreaList[i].EntryArea(true);
                StageManager.Instance.OnReStartArea();
            }

            yield return new WaitUntil(() => stageAreaList[i].isAreaClear);

            StageManager.Instance.InputLock = true;
            TimerManager.Instance.ChangeOnTimer(false);

            #region 수집품 저장
            SaveDataManager.Instance.SaveCollectionJSON();
            #endregion

            if (i == stageAreaList.Count - 1) //마지막 구역의 끝(스테이지 클리어 시)
            {
                StageManager.Instance.fadeImg.gameObject.SetActive(true);
                EndManager.Instance.EndVolume();
                yield return new WaitForSeconds(1.5f);
                StageManager.Instance.fadeImg.gameObject.SetActive(false);
                TimerManager.Instance.EndRewind();
                break;
            }
            TimerManager.Instance.EndRewind();
            //yield return new WaitForSeconds(.5f);
        }
        if (CollectionManager.Instance)
            CollectionManager.Instance.SaveClearCollection();

        if (ClearManager.Instance)
            ClearManager.Instance.SaveClearData();

        if (EndManager.Instance)
            EndManager.Instance.End();
    }
}
