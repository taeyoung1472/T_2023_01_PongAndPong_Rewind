using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DotweenTestJaeby : MonoBehaviour
{
    [SerializeField]
    private Transform _testObj = null;
    [SerializeField]
    private Transform _camObj = null;
    [SerializeField]
    private Vector3 _punch = Vector3.left;
    [SerializeField]
    private float _time = 0.4f;

    private Sequence _seq = null;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Rotate(_testObj, _punch);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Rotate(_testObj, _punch);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Rotate(_camObj, _punch);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Rotate(_camObj, _punch);
        }
    }

    private void Rotate(Transform target, Vector3 punch)
    {
        if (_seq != null)
            _seq.Kill();
        target.transform.rotation = Quaternion.identity;
        _seq = DOTween.Sequence();
        _seq.Append(target.DORotate(punch * 10f, _time)).SetLoops(2, LoopType.Yoyo);

    }
}
