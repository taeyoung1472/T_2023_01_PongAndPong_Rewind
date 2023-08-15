using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabCollectionObjController : MonoBehaviour
{
    private List<LabCollectionObj> _labCollectionObjs = new List<LabCollectionObj>();

    private void Start()
    {
        _labCollectionObjs.AddRange(transform.GetComponentsInChildren<LabCollectionObj>());
    }

    [ContextMenu("테스트 시작맨")]
    public void TestPercentSet()
    {
        for (int i = 0; i < _labCollectionObjs.Count; i++)
        {
            _labCollectionObjs[i].CollectPercentSet();
        }
    }
}
