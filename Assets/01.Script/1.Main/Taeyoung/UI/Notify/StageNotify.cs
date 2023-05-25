using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageNotify : MonoBehaviour
{
    [SerializeField] private string[] notifyDataArr;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < notifyDataArr.Length; i++)
        {
            NotifyManager.Instance.Notify(notifyDataArr[i]);
            yield return new WaitForSeconds(notifyDataArr[i].Length * 0.075f);
        }
    }
}
