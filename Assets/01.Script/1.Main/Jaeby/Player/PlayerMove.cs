using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMove : PlayerAction
{
    [SerializeField]
    private ParticleSystem _dustParticle = null;
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
        amount *= GetSlowRatio();

        if (_player.PlayerAnimation.MoveFlipLock)
            amount *= dir.x; // 회전해서 공격이 문제면 여기가 문제야
        else
            amount *= (_player.PlayerRenderer.Fliped ? -1f : 1f);

        if (_player.PlayerRenderer.GetHorizontalFlip())
            _player.VelocitySetMove(x: amount);
        else
            _player.VelocitySetMove(y: -amount);

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
            yield return new WaitForSeconds(_player.playerMovementSO.moveAudioCooltime);
        }
    }

    public float GetSlowRatio()
    {
        float ratio = 1f;
        if (_player.playerBuff.BuffCheck(PlayerBuffType.PushSlow))
            ratio *= _player.playerMovementSO.pushSlowSpeed;
        if (_player.playerBuff.BuffCheck(PlayerBuffType.Slow))
            ratio *= _player.playerMovementSO.slowSpeed;
        return ratio;
    }
}
