using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMove : PlayerAction
{
    [SerializeField]
    private float _slowSpeed = 0.5f;
    [SerializeField]
    private float _pushSlowSpeed = 0.3f;

    [SerializeField]
    private ParticleSystem _dustParticle = null;
    [SerializeField]
    private float _moveAudioCooltime = 0.1f;
    private Coroutine _moveAudioCoroutine = null;

    [SerializeField]
    private UnityEvent<Vector2> OnMove = null;

    private void Update()
    {
        if (_locked)
        {
            return;
        }
        Vector2 moveInputVector = _player.PlayerInput.InputVector;

        Move(moveInputVector);
    }

    public void Move(Vector2 dir)
    {
        if (_player.playerBuff.BuffCheck(PlayerBuffType.PushSlow))
            dir *= _pushSlowSpeed;
        if (_player.playerBuff.BuffCheck(PlayerBuffType.Slow))
            dir *= _slowSpeed;

        _player.VelocitySetMove(x: dir.x * _player.playerMovementSO.speed);
        _excuting = Mathf.Abs(dir.x) > 0f;
        if (_excuting && _player.IsGrounded && _player.playerBuff.BuffCheck(PlayerBuffType.PushSlow) == false && _player.playerBuff.BuffCheck(PlayerBuffType.Slow) == false)
        {
            if (_dustParticle.isPlaying == false)
            {
                _dustParticle.Play();
                _moveAudioCoroutine = StartCoroutine(MoveAudioCoroutine());
            }
        }
        else
        {
            if (_dustParticle.isPlaying)
            {
                _dustParticle.Stop();
                if (_moveAudioCoroutine != null)
                    StopCoroutine(_moveAudioCoroutine);
            }
        }
        OnMove?.Invoke(dir);
    }


    public override void ActionExit()
    {
        _excuting = false;
        _dustParticle.Stop();
        _player.VelocitySetMove(x: 0f);
        if (_moveAudioCoroutine != null)
            StopCoroutine(_moveAudioCoroutine);
    }

    private IEnumerator MoveAudioCoroutine()
    {
        while (true)
        {
            _player.playerAudio.MoveAudio();
            yield return new WaitForSeconds(_moveAudioCooltime);
        }
    }
}
