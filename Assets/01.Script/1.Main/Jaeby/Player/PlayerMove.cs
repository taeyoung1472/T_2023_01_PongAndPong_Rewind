using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMove : PlayerAction
{
    private float slowSpeed =0.5f;
    public bool isSlow = false;
    private void Update()
    {
        if (_locked)
        {
            return;
        }
        if (!isSlow)
        {
            Move(_player.PlayerInput.InputVector);
        }
        else
        {
            SlowMove(_player.PlayerInput.InputVector);
        }
    }

    public void Move(Vector2 dir)
    {
        _player.VelocitySetMove(x: dir.x * _player.playerMovementSO.speed);
        _excuting = Mathf.Abs(dir.x) > 0f;
    }

    public void SlowMove(Vector2 dir)
    {
        _player.VelocitySetMove(x: dir.x *  slowSpeed * _player.playerMovementSO.speed);
        _excuting = Mathf.Abs(dir.x) > 0f;
    }


    public override void ActionExit()
    {
        _excuting = false;
        _player.VelocitySetMove(x: 0f);
    }

}
