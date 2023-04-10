using System.Collections.Generic;
using System.Linq;
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

    private void OnCollisionExit(Collision collision)
    {
        if (_pushingCollider.Count == 0)
        {
            _excuting = false;
            return;
        }

        if (_pushingCollider.Contains(collision))
        {
            _pushingCollider.Remove(collision);
            _pushingHashs.Remove(collision.gameObject.GetHashCode());
            Debug.Log("오브젝트 밀기 빠져나감");
            OnExitCollider?.Invoke();
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
