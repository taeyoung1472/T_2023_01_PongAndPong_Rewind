using Michsky.UI.Shift;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlatform : GimmickObject
{
    [SerializeField, Header("null이면 root")]
    private Transform _parentTrm = null;
    public Transform ParentTrm { get => _parentTrm; set => _parentTrm = value; }

    private Dictionary<GameObject, MovePlatformTargetData> _targetDictionary = new Dictionary<GameObject, MovePlatformTargetData>();

    [SerializeField]
    private LayerMask _mask = 0;
    [SerializeField]
    private bool _larpMoving = false;
    [SerializeField]
    private bool _childMoving = true;
    [SerializeField]
    private float _lerpSpeed = 0.5f;
    private Vector3 _lastPosition = Vector3.zero;
    private BoxCollider _col = null;

    private void Start()
    {
        if (_parentTrm == null)
            _parentTrm = transform.root;
        _col = _parentTrm.GetComponent<BoxCollider>();
        if (_col == null)
            Debug.LogWarning("Moveplatform : ParentTrm에 BoxCollider 존재하지 않음.");
    }

    public override void InitOnPlay()
    {
        base.InitOnPlay();
        _lastPosition = Vector3.zero;
        TargetsReset();
    }

    public override void InitOnRewind()
    {
        base.InitOnRewind();
        _lastPosition = _parentTrm.position;
        TargetsReset();
    }

    private void TargetsReset()
    {
        foreach (MovePlatformTargetData data in _targetDictionary.Values)
        {
            TargetTransformRestore(data.obj, false);
        }
        _targetDictionary.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_mask & (1 << other.gameObject.layer)) == 0 || _targetDictionary.ContainsKey(other.gameObject))
            return;

        MovePlatformTargetData target = new MovePlatformTargetData
        {
            obj = other.gameObject,
            lastParent = other.transform.parent,
            lastScale = other.transform.localScale
        };
        _targetDictionary.Add(other.gameObject, target);
        other.transform.SetParent(_parentTrm);
    }

    private void OnTriggerExit(Collider other)
    {
        if ((_mask & (1 << other.gameObject.layer)) == 0 || _targetDictionary.ContainsKey(other.gameObject) == false)
            return;

        TargetTransformRestore(other.gameObject, true);
    }

    private void TargetTransformRestore(GameObject obj, bool remove)
    {
        if (_targetDictionary.ContainsKey(obj) == false)
            return;

        MovePlatformTargetData data = _targetDictionary[obj];
        obj.transform.SetParent(data.lastParent);
        obj.transform.localScale = data.lastScale;
        if(remove)
            _targetDictionary.Remove(obj);
    }

    private void LateUpdate()
    {
        if (isRewind == false)
            return;

        if (_larpMoving)
            _parentTrm.position = Vector3.Lerp(_lastPosition, _parentTrm.position, _lerpSpeed);
        //_rigid.MovePosition();
        if (_childMoving)
        {
            foreach(MovePlatformTargetData data in _targetDictionary.Values)
            {
                if(data.obj.CompareTag("Player"))
                {
                    if (data.obj.GetComponent<Player>().PlayerActionCheck(PlayerActionType.Jump))
                    {
                        continue;
                    }
                }
                Vector3 newVec = data.obj.transform.position;
                if(_col != null)
                    newVec.y = _parentTrm.position.y + _col.size.y * 0.49f;

                data.obj.transform.position = newVec;
            }
        }

        _lastPosition = _parentTrm.position;
    }

    public override void Init()
    {
    }
}

public struct MovePlatformTargetData
{
    public GameObject obj;
    public Transform lastParent;
    public Vector3 lastScale;
}
