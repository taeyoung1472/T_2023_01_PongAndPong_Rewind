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

    private Animator _currentTextAnimator = null;
    private float _nextTextYPos = 0f;

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
        for (int i = 0; i < _dataSO.communicationDatas.Count; i++)
        {
            if (_dataSO.communicationDatas[i].isReset)
            {
                _nextTextYPos = 0f;
                DestroyChildren(_parentTrm);
                yield return new WaitForSeconds(_dataSO.animationTime * 1.2f);
            }
            CommunicationUIPrefab prefab = Instantiate(_prefab, _parentTrm);
            prefab.SetUI(_dataSO.communicationDatas[i].communicationSprite, _dataSO.communicationDatas[i].content);
            if (_dataSO.communicationDatas[i].isReset || _currentTextAnimator == null)
            {
                _currentTextAnimator = prefab.FaceImageAnimator;
            }
            prefab.StartTextAnimation(_currentTextAnimator);
            prefab.AnimationUI(0f, 1f, _dataSO.animationTime);
            Vector2 prefabPosition = prefab.GetComponent<RectTransform>().anchoredPosition;
            prefabPosition.y = _nextTextYPos;
            prefab.GetComponent<RectTransform>().anchoredPosition = prefabPosition;
            _nextTextYPos += prefab.GetTextHeight();
            Debug.Log(_nextTextYPos);
            yield return new WaitForSeconds(_dataSO.communicationDatas[i].nextContentTime);
        }
        DestroyChildren(_parentTrm);
    }

    private void DestroyChildren(Transform parent)
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
            children[i].GetComponent<CommunicationUIPrefab>().AnimationUI(1f, 0f, _dataSO.animationTime, () => Destroy(childObj));
        }
    }
}
