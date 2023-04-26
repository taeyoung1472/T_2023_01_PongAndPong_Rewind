using Michsky.UI.Shift;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    private Vector3 _lastPosition = Vector3.zero;
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
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
            Debug.Log(_targetRigids[i].gameObject.name);
            _targetRigids[i].MovePosition(_targetRigids[i].transform.position + distance);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (_targetRigids.Contains(collision.collider.GetComponent<Rigidbody>()) == false)
            _targetRigids.Add(collision.collider.GetComponent<Rigidbody>());
    }

    private void OnCollisionExit(Collision collision)
    {
        if (_targetRigids.Contains(collision.collider.GetComponent<Rigidbody>()))
            _targetRigids.Remove(collision.collider.GetComponent<Rigidbody>());
    }

    public void RemoveRigid(Rigidbody rigid)
    {
        if (_targetRigids.Contains(rigid) == false)
            _targetRigids.Remove(rigid);
    }

    public void AddRigid(Rigidbody rigid)
    {
        if (_targetRigids.Contains(rigid))
            _targetRigids.Add(rigid);
    }
}
