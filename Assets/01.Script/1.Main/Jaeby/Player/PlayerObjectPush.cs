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

    [SerializeField]
    private float _pushPower = 2f;

    public override void ActionExit()
    {
        _excuting = false;
        _pushingCollider = null;
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
            Debug.Log("������Ʈ �б� ����");
            OnEnterCollider?.Invoke();
        }
        else
        {
            if(_pushingCollider != null)
            {
                _pushingCollider = null;
                _excuting = false;
                Debug.Log("������Ʈ �б� ��������");
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
