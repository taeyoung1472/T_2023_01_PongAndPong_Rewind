using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TrailManager : MonoSingleTon<TrailManager>
{
    [SerializeField] private TrailSequence[] sequenceArray;

    private bool isReadyToNextSequence;
    private int curSequenceIndex = 0;

    public void Start()
    {
        StartCoroutine(SequenceCor());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isReadyToNextSequence = true;
        }
    }

    public void Print(string s)
    {
        Debug.Log(s);
    }

    IEnumerator SequenceCor()
    {
        while (curSequenceIndex != sequenceArray.Length)
        {
            yield return new WaitUntil(() => isReadyToNextSequence);
            isReadyToNextSequence = false;
            bool isAppend = false;
            float appendTime = 0.0f;

            for (int i = 0; i < sequenceArray[curSequenceIndex].unitArray.Length; i++)
            {
                TrailSequenceUnit curSeqUnit = sequenceArray[curSequenceIndex].unitArray[i];

                switch (curSeqUnit.seqType)
                {
                    case SequenceType.Append:
                        if (isAppend) yield return new WaitForSeconds(appendTime);
                        isAppend = true;
                        curSeqUnit.callEvent?.Invoke();
                        appendTime = curSeqUnit.time;
                        break;
                    case SequenceType.Join:
                        curSeqUnit.callEvent?.Invoke();
                        break;
                    case SequenceType.AppendInterval:
                        if (isAppend) yield return new WaitForSeconds(appendTime);
                        yield return new WaitForSeconds(curSeqUnit.time);
                        isAppend = false;
                        break;
                }
            }

            curSequenceIndex++;

            if (curSequenceIndex == sequenceArray.Length)
            {
                break;
            }
        }

        Print("End");
    }

    [System.Serializable]
    public struct TrailSequence
    {
        public TrailSequenceUnit[] unitArray;
    }

    [System.Serializable]
    public struct TrailSequenceUnit
    {
        public SequenceType seqType;
        public UnityEvent callEvent;
        public float time;
    }
}