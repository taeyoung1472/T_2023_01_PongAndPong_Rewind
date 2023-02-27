using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectManager : MonoBehaviour
{
    [SerializeField] private RectTransform topBar;
    [SerializeField] private RectTransform bottomBar;

    [SerializeField] private List<DirectUnit> directList = new();
    public List<DirectUnit> DirectList { get { return directList; } set { directList = value; } }

    public void Start()
    {
        StartCoroutine(SequenceCor());
    }

    public void Log(string msg)
    {
        Debug.Log($"{msg} : {Time.time}");
    }

    IEnumerator SequenceCor()
    {
        topBar.DOSizeDelta(new Vector2(topBar.sizeDelta.x, Screen.height / 10), 1);
        bottomBar.DOSizeDelta(new Vector2(bottomBar.sizeDelta.x, Screen.height / 10), 1);

        bool isAppend = false;
        float appendTime = 0.0f;

        for (int i = 0; i < directList.Count; i++)
        {
            switch (directList[i].sequenceType)
            {
                case SequenceType.Append:
                    if (isAppend)
                    {
                        yield return new WaitForSeconds(appendTime);
                    }
                    isAppend = true;
                    directList[i].unityEvent?.Invoke();
                    appendTime = directList[i].time;
                    break;
                case SequenceType.Join:
                    directList[i].unityEvent?.Invoke();
                    break;
                case SequenceType.AppendInterval:
                    if (isAppend)
                    {
                        yield return new WaitForSeconds(appendTime);
                    }
                    yield return new WaitForSeconds(directList[i].time);
                    isAppend = false;
                    break;
            }
        }
    }
}
