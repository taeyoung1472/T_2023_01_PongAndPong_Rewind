using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGimmickObject : GimmickObject
{
    private Player _player = null;

    public override void AddForce(Vector3 dir, float force, ForceMode forceMode = ForceMode.Impulse)
    {
        _player.GetPlayerAction<PlayerJump>().JumpCountUp();
        _player.GetPlayerAction<PlayerJump>().ForceJump(dir, force);
    }

    public override void Init()
    {
        if (_player == null)
            _player = GetComponent<Player>();
        _player.ForceStop();
        Debug.Log("พร");
    }

    public override void RecordTopPosition()
    {
        if (_player.characterController.velocity.y >= -1f)
        {
            recordPosY = transform.position.y;
        }
    }
}
