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
        if (isReStart) //���� ����ŸƮ�� ���� �ְ� 
        {
            BreakScreenController.Instance.StartBreakScreen();
        }
        else //�׳� ���������̸� ���� �ٸ��ų� �ȳְ�
        {

        }
        yield return new WaitUntil(()=> !BreakScreenController.Instance.isBreaking);

        yield return new WaitForSeconds(0.5f);

        if (TimerManager.Instance.isRewinding) //���� �����ε� ���¿��� ������Ҷ�
        {
            TimerManager.Instance.EndRewind();
        }
        else // �Ϲ� ���� �ð��� ����� �Ҷ�
        {
            Debug.Log("����Ÿ��");
            StageManager.Instance.InitPlayer(false);
            curArea.EntryArea();
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
