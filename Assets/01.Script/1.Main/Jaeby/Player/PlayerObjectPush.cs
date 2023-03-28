using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerObjectPush : PlayerAction
{
    private List<ControllerColliderHit> _hits = new List<ControllerColliderHit>();
    private List<int> _hitsHashs = new List<int>();
    [SerializeField]
    private UnityEvent<List<ControllerColliderHit>> OnEnterCollider = null;
    [SerializeField]
    private UnityEvent<List<ControllerColliderHit>> OnExitCollider = null;

    [SerializeField]
    private float _pushPower = 2f;
    [SerializeField]
    private float _littleBitMore = 0.2f;

    public override void ActionExit()
    {
        _excuting = false;
        _hits.Clear();
        _hitsHashs.Clear();
        OnExitCollider?.Invoke(_hits);
    }

    private void Update()
    {
        if (_hits.Count == 0)
        {
            _excuting = false;
            return;
        }

        List<ControllerColliderHit> removeList = null;
        foreach (var hit in _hits)
        {
            Vector3 distanceDir = (hit.transform.position - _player.transform.position).normalized;
            Vector3 dir;
            float distance;
            if (Physics.ComputePenetration(_player.characterController, _player.transform.position + (distanceDir * _littleBitMore), _player.transform.rotation,
                hit.collider, hit.transform.position, hit.transform.rotation, out dir, out distance) == false) // �浹 ��
            {
                if (removeList == null)
                    removeList = new List<ControllerColliderHit>();
                removeList.Add(hit);
                Debug.Log("Collder Exit");
                OnExitCollider?.Invoke(_hits);
            }
        }
        if (removeList != null)
        {
            for (int i = 0; i < removeList.Count; i++)
            {
                _hitsHashs.Remove(removeList[i].gameObject.GetHashCode());
                _hits.Remove(removeList[i]);
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_locked)
            return;

        Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
        if (rb == null)
            return;
        Vector3 forceDir = (hit.gameObject.transform.position - transform.position).normalized;
        rb.AddForce(forceDir * _pushPower, ForceMode.Force);
        //rb.AddForceAtPosition(forceDir, transform.position, ForceMode.Impulse);
        if (_hitsHashs.Contains(hit.gameObject.GetHashCode()) == false)
        {
            _hitsHashs.Add(hit.gameObject.GetHashCode());
            Debug.Log("Collder Enter");
            _excuting = true;
            _hits.Add(hit);
            OnEnterCollider?.Invoke(_hits);
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
