using System;
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

    private bool _lastExcuting = false;
    private float _acelRatio = 0f;
    private float _acelTime = 0f;

    private void Update()
    {
        if (_locked)
        {
            return;
        }

        Vector2 moveInputVector = _player.PlayerInput.RotatedInputVector;
        Move(moveInputVector);
    }

    public void Move(Vector2 dir)
    {
        float amount = _player.playerMovementSO.speed * _acelRatio;
        if (_player.playerBuff.BuffCheck(PlayerBuffType.PushSlow))
            amount *= _pushSlowSpeed;
        if (_player.playerBuff.BuffCheck(PlayerBuffType.Slow))
            amount *= _slowSpeed;
        amount *= (_player.PlayerRenderer.Fliped ? -1f : 1f);

        if (_player.PlayerRenderer.GetHorizontalFlip())
            _player.VelocitySetMove(x: amount);
        else
            _player.VelocitySetMove(y: amount);

        _excuting = dir.sqrMagnitude > 0f;
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

        if (_excuting)
            _acelTime += (1 / _player.playerMovementSO.accelerationTime) * Time.deltaTime;
        else
            _acelTime -= (1 / _player.playerMovementSO.decelerationTime) * Time.deltaTime;

        _acelTime = Mathf.Clamp(_acelTime, 0f, 1f);
        _acelRatio = Mathf.Sin((float)(_acelTime * Math.PI) / 2f);

        if (_lastExcuting == _excuting)
            return;
        _lastExcuting = _excuting;
        if (_lastExcuting)
        {
            AcelReset();
        }
    }

    public void OnFlipMove(bool value)
    {
        AcelReset();
    }

    public void AcelReset()
    {
        _acelTime = 0f;
        _acelRatio = 0f;
    }

    public override void ActionExit()
    {
        _excuting = false;
        _dustParticle.Stop();
        _player.VelocitySetMove(0f, 0f);
        AcelReset();
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
