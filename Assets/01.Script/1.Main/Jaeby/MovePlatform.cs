using Michsky.UI.Shift;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField, Header("타겟과 y 비교용")]
    private Transform _topPosition = null;

    private Vector3 _lastPosition = Vector3.zero;
    private List<Rigidbody> _targetRigids = new List<Rigidbody>();

    private void Start()
    {
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


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.position.y < _topPosition.position.y)
            return;

        Rigidbody rigid = null;
        collision.collider.gameObject.TryGetComponent<Rigidbody>(out rigid);
        if (rigid != null)
            AddRigid(rigid);
    }

    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rigid = null;
        collision.collider.gameObject.TryGetComponent<Rigidbody>(out rigid);
        if (rigid != null)
            RemoveRigid(rigid);
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
}
