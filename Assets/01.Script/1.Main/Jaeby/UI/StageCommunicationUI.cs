using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCommunicationUI : MonoSingleTon<StageCommunicationUI>
{
    [SerializeField]
    private StageCommunicationSO _dataSO = null;
    [SerializeField]
    private CommunicationUIPrefab _prefab = null;
    private Transform _parentTrm = null;

    private Animator _currentTextAnimator = null;
    private float _nextTextYPos = 0f;

    private Coroutine _curCommuCoroutine = null;

    private void Start()
    {
        _parentTrm = transform.Find("ParentTrm");
        CommunicationStart();
    }

    public void CommunicationStart(StageCommunicationSO dataSO)
    {
        if (dataSO == null)
            return;
        if(_curCommuCoroutine != null)
        {
            StopCoroutine(_curCommuCoroutine);
        }
        _curCommuCoroutine = StartCoroutine(CommunicationCoroutine(dataSO));
    }

    public void CommunicationStart()
    {
        if (_dataSO == null)
            return;
        if (_curCommuCoroutine != null)
        {
            StopCoroutine(_curCommuCoroutine);
        }
        _curCommuCoroutine = StartCoroutine(CommunicationCoroutine(_dataSO));
    }

    private IEnumerator CommunicationCoroutine(StageCommunicationSO dataSO)
    {
        DestroyChildren(_parentTrm, dataSO);
        for (int i = 0; i < dataSO.communicationDatas.Count; i++)
        {
            if (dataSO.communicationDatas[i].isReset)
            {
                _nextTextYPos = 0f;
                DestroyChildren(_parentTrm, dataSO);
                yield return new WaitForSeconds(dataSO.animationTime * 1.2f);
            }
            CommunicationUIPrefab prefab = Instantiate(_prefab, _parentTrm);
            prefab.SetUI(dataSO.communicationDatas[i].communicationSprite, dataSO.communicationDatas[i].content);
            if (dataSO.communicationDatas[i].isReset || _currentTextAnimator == null)
            {
                _currentTextAnimator = prefab.FaceImageAnimator;
            }
            prefab.StartTextAnimation(_currentTextAnimator);
            prefab.AnimationUI(0f, 1f, dataSO.animationTime);
            Vector2 prefabPosition = prefab.GetComponent<RectTransform>().anchoredPosition;
            prefabPosition.y = _nextTextYPos;
            prefab.GetComponent<RectTransform>().anchoredPosition = prefabPosition;
            _nextTextYPos += prefab.GetTextHeight();
            yield return new WaitForSeconds(dataSO.communicationDatas[i].nextContentTime);
        }
        DestroyChildren(_parentTrm, dataSO);
    }

    private void DestroyChildren(Transform parent, StageCommunicationSO dataSO)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parent)
        {
            children.Add(child.gameObject);
        }
        for (int i = 0; i < children.Count; i++)
        {
            children[i].transform.SetParent(transform);
            GameObject childObj = children[i];
            children[i].GetComponent<CommunicationUIPrefab>().AnimationUI(1f, 0f, dataSO.animationTime, () => Destroy(childObj));
        }
    }
}