using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectController : MonoBehaviour
{
    [SerializeField] private List<DirectUnit> directList;
    public List<DirectUnit> DirectList { get { return directList; } set { directList = value; } }
    [SerializeField] private int priority;
    public int Priority { get { return priority; } set { priority = value; } }
    private bool isEnd = false;
    public bool IsEnd { get { return isEnd; } }

    public void Active()
    {
        StartCoroutine(SequenceCor());
    }

    IEnumerator SequenceCor()
    {
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

        isEnd = true;
    }
}
