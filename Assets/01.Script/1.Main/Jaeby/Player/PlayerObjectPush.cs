using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Highlighters.HighlighterTrigger;

public class PlayerObjectPush : PlayerAction
{
    [SerializeField]
    private UnityEvent OnEnterCollider = null;
    [SerializeField]
    private UnityEvent OnExitCollider = null;

    private List<Collision> _pushingCollider = new List<Collision>();
    private List<int> _pushingHashs = new List<int>();

    [SerializeField]
    private float _pushPower = 2f;
    [SerializeField]
    private float _littleBitMore = 0.2f;

    public override void ActionExit()
    {
        _excuting = false;
        _pushingCollider.Clear();
        _pushingHashs.Clear();
        OnExitCollider?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_locked)
            return;
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        if (rb == null)
            return;
        Vector3 forceDir = (collision.gameObject.transform.position.x > transform.position.x) ? Vector3.right : Vector2.left;
        rb.AddForce(forceDir * _pushPower, ForceMode.Force);

        if (_pushingHashs.Contains(collision.GetHashCode()) == false)
        {
            _excuting = true;
            _pushingHashs.Add(collision.gameObject.GetHashCode());
            Debug.Log("오브젝트 밀기 시작");
            _pushingCollider.Add(collision);
            OnEnterCollider?.Invoke();
        }
    }

    private void Update()
    {
        if (_pushingCollider.Count == 0)
        {
            _excuting = false;
            return;
        }

        List<Collision> removeList = null;

        for (int i = 0; i < _pushingCollider.Count; i++)
        {
            Vector3 distanceDir = (_pushingCollider[i].transform.position.x > transform.position.x) ? Vector3.right : Vector2.left;
            Vector3 dir = Vector3.zero;
            float distance = 0f;
            bool result = Physics.ComputePenetration(_player.Col, _player.transform.position + (distanceDir * _littleBitMore), _player.transform.rotation,
                _pushingCollider[i].collider, _pushingCollider[i].transform.position, _pushingCollider[i].transform.rotation, out dir, out distance);

            if (result == false || Vector3.Dot(_player.PlayerRenderer.Forward, _pushingCollider[i].transform.position - _player.transform.position) < 0f)
            {
                if (removeList == null)
                    removeList = new List<Collision>();
                removeList.Add(_pushingCollider[i]);
                Debug.Log("오브젝트 밀기 빠져나감");
                OnExitCollider?.Invoke();
            }
        }
        if(removeList != null)
        {
            for(int i = 0; i < removeList.Count; i++)
            {
                _pushingHashs.Remove(removeList[i].gameObject.GetHashCode());
                _pushingCollider.Remove(removeList[i]);
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
