using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Highlighters.HighlighterTrigger;

public class PlayerObjectPush : PlayerAction
{
    private GameObject _pushingCollider = null;

    [SerializeField]
    private UnityEvent OnEnterCollider = null;
    [SerializeField]
    private UnityEvent OnExitCollider = null;

    public override void ActionExit()
    {
        _excuting = false;
        _pushingCollider = null;
        OnExitCollider?.Invoke();
    }

    private void Update()
    {
        ObjExitCheck();
    }

    private void ObjExitCheck()
    {
        if (_pushingCollider == null)
            return;
        if(Vector3.Dot(_pushingCollider.transform.position - _player.transform.position, _player.PlayerRenderer.Forward) < 0f)
        {
            _pushingCollider = null;
            Debug.Log("오브젝트 밀기 끝");
            OnExitCollider?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PushTrigger") == false || _pushingCollider != null)
            return;
        _pushingCollider = other.transform.root.gameObject;
        Debug.Log("오브젝트 밀기 시작");
        OnEnterCollider?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PushTrigger") == false || _pushingCollider == null)
            return;
        _pushingCollider = null;
        Debug.Log("오브젝트 밀기 끝");
        OnExitCollider?.Invoke();
    }

    public void PushSlowBuff(bool val)
    {
        if (val)
            _player.playerBuff.AddBuff(PlayerBuffType.PushSlow);
        else
            _player.playerBuff.DeleteBuff(PlayerBuffType.PushSlow);
    }
}
