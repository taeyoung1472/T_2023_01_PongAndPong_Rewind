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
        _parentTrm = transform;
    }

    public void CommunicationStart()
    {
        StartCoroutine(CommunicationCoroutine());
    }

    private IEnumerator CommunicationCoroutine()
    {
        for(int i =0; i < _dataSO.communicationDatas.Count; i++)
        {
            yield return new WaitForSeconds(_dataSO.communicationDatas[i].nextContentTime);
        }
    }
}
