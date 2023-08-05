using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCommunicationUI : MonoBehaviour
{
    [SerializeField]
    private StageCommunicationSO _dataSO = null;
    [SerializeField]
    private CommunicationUIPrefab _prefab = null;
    private Transform _parentTrm = null;

    private void Start()
    {
        _parentTrm = transform.Find("ParentTrm");
        CommunicationStart();
    }

    public void CommunicationStart()
    {
        if (_dataSO == null)
            return;
        StartCoroutine(CommunicationCoroutine());
    }

    private IEnumerator CommunicationCoroutine()
    {
        for(int i =0; i < _dataSO.communicationDatas.Count; i++)
        {
            if (_dataSO.communicationDatas[i].isReset)
            {
                DestroyChildren(_parentTrm);
            }
            CommunicationUIPrefab prefab = Instantiate(_prefab, _parentTrm);
            prefab.SetUI(_dataSO.communicationDatas[i].communicationSprite, _dataSO.communicationDatas[i].content);
            yield return new WaitForSeconds(_dataSO.communicationDatas[i].nextContentTime);
        }
    }

    private void DestroyChildren(Transform parent)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parent)
        {
            children.Add(child.gameObject);
        }
        for(int i = 0; i < children.Count; i++)
        {
            Destroy(children[i]);
        }
    }
}
