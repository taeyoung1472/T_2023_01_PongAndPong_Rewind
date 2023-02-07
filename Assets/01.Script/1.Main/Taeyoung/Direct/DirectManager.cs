using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectManager : MonoBehaviour
{
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
