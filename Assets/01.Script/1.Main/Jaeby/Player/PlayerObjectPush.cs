using UnityEngine;
using UnityEngine.Events;

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
        MassChange();
        ObjExitCheck();
    }

    private void MassChange()
    {
        if (_pushingCollider != null)
        {
            if (_player.PlayerActionCheck(PlayerActionType.Dash))
            {
                Debug.Log("대시");
                _pushingCollider.GetComponentInParent<Rigidbody>().mass = 90f;
            }
            else
            {
                Debug.Log("아니");
                _pushingCollider.GetComponentInParent<Rigidbody>().mass = 1f;
            }
            Debug.Log(_pushingCollider.GetComponentInParent<Rigidbody>().mass);
        }
    }

    private void PushEnd()
    {
        _pushingCollider = null;
        Debug.Log("오브젝트 밀기 끝");
        OnExitCollider?.Invoke();
        _excuting = false;
    }

    private void ObjExitCheck()
    {
        if (_pushingCollider == null)
            return;
        if (Vector3.Dot(_pushingCollider.transform.position - _player.transform.position, _player.PlayerRenderer.Forward) < 0f)
        {
            PushEnd();
        }
    }

    public void PushStart(GameObject other)
    {
        if (other.CompareTag("PushTrigger") == false || _pushingCollider != null  )
            return;
        _pushingCollider = other.transform.gameObject;
        MassChange();
        Debug.Log("오브젝트 밀기 시작");
        OnEnterCollider?.Invoke();
        _excuting = true;
    }

    public void PushEnd(GameObject other)
    {
        if (other.CompareTag("PushTrigger") == false || _pushingCollider == null)
            return;
        MassChange();
        PushEnd();
    }

    public void PushSlowBuff(bool val)
    {
        if (val)
            _player.playerBuff.AddBuff(PlayerBuffType.PushSlow);
        else
            _player.playerBuff.DeleteBuff(PlayerBuffType.PushSlow);
    }
}
