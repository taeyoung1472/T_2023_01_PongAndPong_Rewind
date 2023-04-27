using Michsky.UI.Shift;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField]
    private bool _isTrigger = false;
    [SerializeField, Header("null이면 자신 trm")]
    private Transform _centerPostiion = null;
    [SerializeField, Header("상하좌우만 건드셈")]
    private List<DirectionType> _checkDirections = new List<DirectionType>();

    private Vector3 _lastPosition = Vector3.zero;
    private List<Rigidbody> _targetRigids = new List<Rigidbody>();

    private void Start()
    {
        if (_centerPostiion == null)
            _centerPostiion = transform;

        _lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        Vector3 distance = transform.position - _lastPosition;
        _lastPosition = transform.position;

        for (int i = 0; i < _targetRigids.Count; i++)
        {
            //_targetRigids[i].MovePosition(_targetRigids[i].transform.position + distance * 1.4f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isTrigger == false)
            return;
        Enter(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isTrigger == false)
            return;
        Exit(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isTrigger)
            return;
        Enter(collision.collider.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (_isTrigger)
            return;
        Exit(collision.collider.gameObject);
    }

    public void RemoveRigid(Rigidbody rigid)
    {
        rigid.transform.SetParent(null);
        //if (_targetRigids.Contains(rigid))
        //    _targetRigids.Remove(rigid);
    }

    public void AddRigid(Rigidbody rigid)
    {
        rigid.transform.SetParent(transform);
        //if (_targetRigids.Contains(rigid) == false)
        //    _targetRigids.Add(rigid);
    }
    
    private void Enter(GameObject obj)
    {
        Vector2 cp = obj.transform.position;
        Vector2 mp = _centerPostiion.position;
        if (CaseCheck(cp, mp))
        {
            Debug.Log($"체크요{obj.name}");
        }
        else
            return;

        Rigidbody rigid = null;
        obj.TryGetComponent<Rigidbody>(out rigid);
        if (rigid != null)
            AddRigid(rigid);
    }

    private void Exit(GameObject obj)
    {
        Rigidbody rigid = null;
        obj.TryGetComponent<Rigidbody>(out rigid);
        if (rigid != null)
            RemoveRigid(rigid);
    }

    private bool CaseCheck(Vector2 cp, Vector2 mp)
    {
        for (int i = 0; i < _checkDirections.Count; i++)
        {
            switch (_checkDirections[i])
            {
                case DirectionType.None:
                    return false;
                case DirectionType.Left:
                    if (cp.x < mp.x)
                        return true;
                    break;
                case DirectionType.Right:
                    if (cp.x > mp.x)
                        return true;
                    break;
                case DirectionType.Up:
                    if (cp.y > mp.y)
                        return true;
                    break;
                case DirectionType.Down:
                    if (cp.y < mp.y)
                        return true;
                    break;
                default:
                    return false;
            }
        }
        return false;
    }
}
