using System;
using UnityEngine;
using UnityEngine.Events;
using static Unity.VisualScripting.Member;

public class PlayerObjectPush : PlayerAction
{
    private GameObject _pushingCollider = null;

    [SerializeField]
    private UnityEvent OnEnterCollider = null;
    [SerializeField]
    private UnityEvent OnExitCollider = null;

    private AudioSource source;
    private bool isPushing;

    protected override void Awake()
    {
        base.Awake();
        source = gameObject.AddComponent<AudioSource>();
        source.clip = AudioManager.DataBase.GetAudio(SoundType.OnDragingObject);
        source.volume = 0;
        source.loop = true;
        source.outputAudioMixerGroup = AudioManager.MixerDic["SFX"];
        source.Play();
    }

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
        Sound();
    }

    private void Sound()
    {
        if (!isPushing)
        {
            source.volume = 0;
        }
        else
        {
            if (_player.Rigid.velocity.magnitude >= 0.2f)
            {
                source.volume = 0.5f;
            }
            else
            {
                source.volume = 0;
            }
        }
    }

    private void MassChange()
    {
        if (_pushingCollider != null)
        {
            if (_player.PlayerActionCheck(PlayerActionType.Dash))
            {
                Debug.Log("���");
                _pushingCollider.GetComponentInParent<Rigidbody>().mass = 90f;
            }
            else
            {
                Debug.Log("�ƴ�");
                _pushingCollider.GetComponentInParent<Rigidbody>().mass = 1f;
            }
            Debug.Log(_pushingCollider.GetComponentInParent<Rigidbody>().mass);
        }
    }

    private void PushEnd()
    {
        isPushing = false;
        _pushingCollider = null;
        Debug.Log("������Ʈ �б� ��");
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

        if (!RewindManager.Instance.IsBeingRewinded)
            isPushing = true;
        else
            return;

        _pushingCollider = other.transform.gameObject;
        MassChange();
        Debug.Log("������Ʈ �б� ����");
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
