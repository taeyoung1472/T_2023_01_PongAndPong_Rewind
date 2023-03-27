using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMove : PlayerAction
{
    private float slowSpeed = 0.5f;
    public bool isSlow = false;
    [SerializeField]
    private ParticleSystem _dustParticle = null;
    [SerializeField]
    private float _moveAudioCooltime = 0.1f;
    private Coroutine _moveAudioCoroutine = null;

    private void Update()
    {
        if (_locked)
        {
            return;
        }
        Vector2 moveInputVector = _player.PlayerInput.InputVector;
        if (isSlow)
            moveInputVector *= slowSpeed;

        Move(moveInputVector);
    }

    public void Move(Vector2 dir)
    {
        _player.VelocitySetMove(x: dir.x * _player.playerMovementSO.speed);
        _excuting = Mathf.Abs(dir.x) > 0f;
        if (_excuting && _player.IsGrounded)
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
        while(true)
        {
            _player.playerAudio.MoveAudio();
            yield return new WaitForSeconds(_moveAudioCooltime);
        }
    }
}
