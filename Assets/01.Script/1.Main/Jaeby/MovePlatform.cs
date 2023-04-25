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

    private void Update()
    {
        transform.Translate(Vector2.up * _speed * Time.deltaTime);
    }

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
        _targetRigids.Add(collision.collider.GetComponent<Rigidbody>());
    }

    private void OnCollisionExit(Collision collision)
    {
        _targetRigids.Remove(collision.collider.GetComponent<Rigidbody>());
    }

}
