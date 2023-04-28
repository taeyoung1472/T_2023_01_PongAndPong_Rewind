using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotifyManager : MonoSingleTon<NotifyManager>
{
    [SerializeField] private Notify notify;
    [SerializeField] private Transform notifyParent;

    VerticalLayoutGroup layoutGroup;
    Queue<Notify> notifyQueue = new();

    public void Update()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CloseNotify();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Notify("안녕하세요 저는 도움말 입니다.");
        }
    }

    public void Notify(string text)
    {
        Notify obj = Instantiate(notify, notifyParent);
        obj.gameObject.SetActive(true);
        obj.SetNotify(text);
        notifyQueue.Enqueue(obj);
    }

    public void CloseNotify()
    {
        if (notifyQueue.Count <= 0)
            return;

        Notify obj = notifyQueue.Dequeue();
        obj.Close();
    }
}
