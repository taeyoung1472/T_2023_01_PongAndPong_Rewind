using Michsky.UI.Shift;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField, Header("null¿Ã∏È root")]
    private Transform _parentTrm = null;

    private List<Transform> _trmList = new List<Transform>();

    private void Start()
    {
        if (_parentTrm == null)
            _parentTrm = transform.root;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() == null)
            return;
        Transform trm = other.transform;
        if (_trmList.Contains(trm))
            return;
        if (other.GetComponent<Player>() != null)
        {
            other.GetComponent<Player>().GravityModule.IsMovePlatform = true;
        }
        _trmList.Add(trm);
        trm.SetParent(_parentTrm);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>() == null)
            return;
        Transform trm = other.transform;
        if (_trmList.Contains(trm) == false)
            return;
        if (other.GetComponent<Player>() != null)
        {
            other.GetComponent<Player>().GravityModule.IsMovePlatform = false;
        }
        _trmList.Remove(trm);
        trm.SetParent(null);
    }
}
