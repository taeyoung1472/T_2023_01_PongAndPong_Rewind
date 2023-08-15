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

    [ContextMenu("�׽�Ʈ ���۸�")]
    public void TestPercentSet()
    {
        for (int i = 0; i < _labCollectionObjs.Count; i++)
        {
            _labCollectionObjs[i].CollectPercentSet();
        }
    }
}
