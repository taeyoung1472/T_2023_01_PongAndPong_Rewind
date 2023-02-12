using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMove : PlayerAction
{
    private void Update()
    {
        if (_locked)
            return;
        Move(_player.PlayerInput.InputVectorNorm);
    }

    public void Move(Vector2 dir)
    {
        _player.VelocitySetMove(x: dir.x * _player.playerMovementSO.speed);
        _excuting = Mathf.Abs(dir.x) > 0f;
    }

    public override void ActionExit()
    {
        _excuting = false;
        _player.VelocitySetMove(x: 0f);
    }

}
