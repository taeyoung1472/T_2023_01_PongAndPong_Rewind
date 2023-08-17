using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabCollectionObjController : MonoBehaviour
{
    private List<LabCollectionObj> _labCollectionObjs = new List<LabCollectionObj>();

    [ContextMenu("테스트 시작맨")]
    public void PercentSet()
    {
        if(_labCollectionObjs.Count == 0)
        {
            _labCollectionObjs.AddRange(transform.GetComponentsInChildren<LabCollectionObj>());
        }
        for (int i = 0; i < _labCollectionObjs.Count; i++)
        {
            _labCollectionObjs[i].CollectPercentSet();
        }
    }
}
