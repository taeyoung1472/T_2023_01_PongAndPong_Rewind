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
            if (!IsRewindMotionTrail)
            {
                TrailManager.Instance.TrailTimerReset(this);
            }
        }
    }

    [SerializeField]
    private bool _isRewindMotionTrail = false;
    public bool IsRewindMotionTrail
    {
        get => _isRewindMotionTrail;
        set
        {
            _isRewindMotionTrail = value;
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

    private void OnEnable()
    {
        TrailEnable();
    }

    public void TrailDisable()
    {
        //Debug.Log($"TrailableObejct 제거 : 객체 이름 {gameObject.name}");
        if (TrailManager.Instance != null)
        {
            TrailParent parent = TrailManager.Instance.GetParent(this);
            if (parent == null)
                return;
            parent.Died = true;
        }
    }

    public void TrailEnable()
    {
        if (TrailManager.Instance != null)
        {
            TrailParent parent = TrailManager.Instance.GetParent(this);
            if (parent == null)
                return;
            parent.Died = false;
        }
    }
}
