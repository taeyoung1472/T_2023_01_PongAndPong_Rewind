using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailableObject : MonoBehaviour
{
    [SerializeField]
    private TrailableData _trailData = null;
    public TrailableData trailData => _trailData;

    [SerializeField]
    private bool _isMotionTrail = false;
    public bool IsMotionTrail
    {
        get => _isMotionTrail;
        set
        {
            _isMotionTrail = value;
            TrailManager.Instance.TrailTimerReset(this);
        }
    }

    private void Start()
    {
        TrailManager.Instance.AddTrailObj(this);
    }

    private void OnDisable()
    {
        TrailDisable();
    }


    public void TrailDisable()
    {
        Debug.Log($"TrailableObejct 제거 : 객체 이름 {gameObject.name}");
        _isMotionTrail = false;
        TrailManager.Instance.DeleteTrailObj(this);
    }
}
