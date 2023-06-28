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
        if (isReStart) //���� ����ŸƮ�� ���� �ְ� 
        {
            BreakScreenController.Instance.StartBreakScreen();
        }
        else //�׳� ���������̸� ���� �ٸ��ų� �ȳְ�
        {

        }
        yield return new WaitUntil(() => !BreakScreenController.Instance.isBreaking);

        yield return new WaitForSeconds(.5f);



        if (TimerManager.Instance.isRewinding) //���� �����ε� ���¿��� ������Ҷ�
        {
            transform.DOKill();
            transform.rotation = Quaternion.Euler(0, 0, 0);
            TimerManager.Instance.EndRewind();
        }
        else // �Ϲ� ���� �ð��� ����� �Ҷ�
        {
            //Debug.Log("����Ÿ��");
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

            #region ����ǰ ����
            SaveDataManager.Instance.SaveCollectionJSON();
            #endregion

            if (i == stageAreaList.Count - 1) //������ ������ ��(�������� Ŭ���� ��)
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
