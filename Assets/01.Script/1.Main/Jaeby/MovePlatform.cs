using Michsky.UI.Shift;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlatform : GimmickObject
{
    [SerializeField, Header("null¿Ã∏È root")]
    private Transform _parentTrm = null;
    private Rigidbody _rigid = null;

    private List<GameObject> _objList = new List<GameObject>();

    [SerializeField]
    private LayerMask _mask = 0;
    [SerializeField]
    private float _lerpSpeed = 0.5f;
    private Vector3 _lastPosition = Vector3.zero;

    private void Start()
    {
        if (_parentTrm == null)
            _parentTrm = transform.root;
        _rigid = GetComponentInParent<Rigidbody>();
    }

    public override void InitOnPlay()
    {
        base.InitOnPlay();
        _lastPosition = Vector3.zero;
        _objList.Clear();
    }

    public override void InitOnRewind()
    {
        base.InitOnRewind();
        _lastPosition = _parentTrm.position;
        _objList.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_mask & (1 << other.gameObject.layer)) == 0 || _objList.Contains(other.gameObject))
            return;

        _objList.Add(other.gameObject);
        other.transform.SetParent(_parentTrm);
    }

    private void OnTriggerExit(Collider other)
    {
        if ((_mask & (1 << other.gameObject.layer)) == 0 || _objList.Contains(other.gameObject) == false)
            return;

        _objList.Remove(other.gameObject);
        other.transform.SetParent(null);
    }

    private void LateUpdate()
    {
        if (isRewind == false)
            return;

        _rigid.MovePosition(Vector3.Lerp(_lastPosition, _parentTrm.position, _lerpSpeed));
        for (int i = 0; i < _objList.Count; i++)
        {
            if (_objList[i].GetComponent<Collider>().CompareTag("Player"))
            {
                if(_objList[i].GetComponent<Player>().PlayerActionCheck(PlayerActionType.Jump))
                {
                    continue;
                }
            }
            Vector3 newVec = _objList[i].transform.position;
            newVec.y = _parentTrm.position.y +
                _parentTrm.transform.localScale.y * 0.49f;
            _objList[i].transform.position = newVec;
        }

        _lastPosition = _parentTrm.position;
    }

    public override void Init()
    {
    }
}
