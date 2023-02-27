using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class DirectManager : MonoSingleTon<DirectManager>
{
    [SerializeField] private RectTransform topBar;
    [SerializeField] private RectTransform bottomBar;

    private List<DirectController> directControllerList = new();

    private bool isActive = false;

    public void Init()
    {
        directControllerList.AddRange(FindObjectsOfType<DirectController>());
        directControllerList = directControllerList.OrderBy(x => x.Priority).ToList();

        int max = int.MinValue;
        for (int i = 0; i < directControllerList.Count; i++)
        {
            if (directControllerList[i].Priority > max)
            {
                max = directControllerList[i].Priority;
            }
            else
            {
                Debug.LogError("DirectController Priorty 확인해");
            }
        }
    }

    public void ActiveDirect()
    {
        StartCoroutine(ActiveDirectCor());
    }

    private IEnumerator ActiveDirectCor()
    {
        if (isActive)
        {
            Debug.LogWarning("이미 연출을 실행중 입니다.");
        }
        else if (directControllerList.Count == 0)
        {
            Debug.LogWarning("더이상 실행할 연출은 없습니다.");
        }
        else
        {
            isActive = true;
            DirectController myController = directControllerList[0];
            directControllerList.RemoveAt(0);

            myController.Active();

            topBar.DOSizeDelta(new Vector2(topBar.sizeDelta.x, Screen.height / 10), 1);
            bottomBar.DOSizeDelta(new Vector2(bottomBar.sizeDelta.x, Screen.height / 10), 1);
            yield return new WaitForSeconds(1);

            yield return new WaitUntil(() => myController.IsEnd);

            topBar.DOSizeDelta(new Vector2(topBar.sizeDelta.x, 0), 1);
            bottomBar.DOSizeDelta(new Vector2(bottomBar.sizeDelta.x, 0), 1);
            yield return new WaitForSeconds(1);
            isActive = false;
        }
    }
}
