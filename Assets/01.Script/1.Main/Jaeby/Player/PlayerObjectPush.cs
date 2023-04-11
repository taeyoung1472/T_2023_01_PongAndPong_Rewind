using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Highlighters.HighlighterTrigger;

public class PlayerObjectPush : PlayerAction
{
    [SerializeField]
    private LayerMask _pushMask = 0;
    [SerializeField]
    private float _rayLength = 1f;

    private Collider _pushingCollider = null;

    [SerializeField]
    private UnityEvent OnEnterCollider = null;
    [SerializeField]
    private UnityEvent OnExitCollider = null;

    private List<Collision> _pushingColliders = new List<Collision>();
    private List<int> _pushingHashs = new List<int>();

    [SerializeField]
    private float _pushPower = 2f;
    [SerializeField]
    private float _littleBitMore = 0.2f;

    public override void ActionExit()
    {
        _excuting = false;
        _pushingColliders.Clear();
        _pushingHashs.Clear();
        OnExitCollider?.Invoke();
    }

    private void FixedUpdate()
    {
        CollisionCheck();
        PushObj();
    }

    private void PushObj()
    {
        if (_pushingCollider == null || _locked)
            return;

        Vector3 distanceDir = (_pushingCollider.transform.position.x > transform.position.x) ? Vector3.right : Vector2.left;
        Rigidbody rb = _pushingCollider.GetComponent<Rigidbody>();
        if (rb == null)
            return;
        rb.AddForce(distanceDir * _pushPower, ForceMode.Force);
    }

    private void CollisionCheck()
    {
        if (_locked)
        {
            _pushingCollider = null;
            return;
        }

        RaycastHit hit;
        bool result = Physics.Raycast(transform.position, _player.PlayerRenderer.Forward, out hit, _rayLength, _pushMask);
        if(result)
        {
            if(_pushingCollider != null)
            {
                if (_pushingCollider == hit.collider)
                    return;
            }

            _pushingCollider = hit.collider;
            _excuting = true;
            Debug.Log("오브젝트 밀기 시작");
            OnEnterCollider?.Invoke();
        }
        else
        {
            if(_pushingCollider != null)
            {
                _pushingCollider = null;
                Debug.Log("오브젝트 밀기 빠져나감");
                OnExitCollider?.Invoke();
            }
        }
    }

    public void PushSlowBuff(bool val)
    {
        if (val)
            _player.playerBuff.AddBuff(PlayerBuffType.PushSlow);
        else
            _player.playerBuff.DeleteBuff(PlayerBuffType.PushSlow);
    }
}
