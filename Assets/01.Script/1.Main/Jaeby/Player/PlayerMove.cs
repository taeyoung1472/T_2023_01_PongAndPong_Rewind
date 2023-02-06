using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerAction
{
    private void Update()
    {
        Move();
    }

    public void Move()
    {
        if (_locked)
            return;
        Vector2 dir = _player.PlayerInput.InputVectorNorm;
        _player.VelocitySet(x: dir.x * _player.playerMovementSO.speed);
        _excuting = Mathf.Abs(dir.x) > 0f;
    }

    public override void ActionExit()
    {
        _excuting = false;
        _player.VelocitySet(x: 0f);
    }
}
