using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayManager : MonoSingleTon<TutorialPlayManager>
{
    [Header("Object")]
    [SerializeField] private GameObject stopVolume;
    [SerializeField] private GameObject stopIcon;

    [Header("List")]
    [SerializeField] private List<NotifyTutoPlayData> notifyList;
    private Dictionary<string, NotifyData> notifyDic = new();

    public void Awake()
    {
        foreach (var item in notifyList)
        {
            notifyDic.Add(item.key, item.value);
        }
    }

    private void OnValidate()
    {
        foreach (var item in notifyList)
        {
            item.name = item.key;
        }
    }

    public void PlayNotify(string key)
    {
        StartCoroutine(DisplayNotify(notifyDic[key]));
    }

    private IEnumerator DisplayNotify(NotifyData data)
    {
        stopVolume.SetActive(true);
        stopIcon.SetActive(true);
        TimerManager.Instance.StopTime();
        for (int i = 0; i < data.notifyString.Length; i++)
        {
            NotifyManager.Instance.Notify(data.notifyString[i]);
            yield return new WaitForSecondsRealtime(data.notifyString[i].Length * 0.1f);
        }
        yield return new WaitUntil(() => NotifyManager.Instance.notifyCount == 0);
        yield return new WaitForSecondsRealtime(1f);
        stopVolume.SetActive(false);
        stopIcon.SetActive(false);
        TimerManager.Instance.ResetFastForwardTime();
    }

    [System.Serializable]
    public class NotifyTutoPlayData
    {
        [HideInInspector] public string name;
        public string key;
        public NotifyData value;
    }

    [System.Serializable]
    public class NotifyData
    {
        public string[] notifyString;
    }
}
