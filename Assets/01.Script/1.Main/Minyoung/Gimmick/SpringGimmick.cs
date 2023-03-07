using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringGimmick : MonoBehaviour
{
    [SerializeField]
    private LayerMask _playerLayer = 0;
    [SerializeField]
    private float _rayLength = 0.2f;
    [SerializeField]
    private float _jumpPower = 5f;
    [SerializeField]
    private float _cooltime = 0.2f;
    private bool _lock = false;

    private Collider _col = null;

    private void Start()
    {
        _col = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        PlayerCheck();
    }

    private void PlayerCheck()
    {
        if (_lock)
            return;
        RaycastHit hit;
        Vector3 boxCenter = _col.bounds.center;
        Vector3 halfExtents = _col.bounds.extents;
        halfExtents.y = _rayLength;
        float maxDistance = _col.bounds.extents.y;
        Physics.BoxCast(boxCenter, halfExtents, Vector3.up, out hit, transform.rotation, maxDistance, _playerLayer);
        if (hit.collider != null)
        {
            Player player = hit.collider.GetComponent<Player>();
            float power = 1f;
            int gravity = Mathf.RoundToInt(player.GravityModule.CurGravityAcceleration / player.GravityModule.MaxGravityAcceleration * 100f);
            if (gravity < 40)
                power = 1f;
            else if (gravity < 80)
                power = 2f;
            else if (gravity <= 100)
                power = 3f;

            StartCoroutine(LockCo());
            PlayerJump playerJump = player.GetPlayerAction(PlayerActionType.Jump) as PlayerJump;
            playerJump.ForceJump(Vector2.up, power * _jumpPower);
            player.GravityModule.OnGrounded(false);
        }
    }

    private IEnumerator LockCo()
    {
        _lock = true;
        yield return new WaitForSeconds(_cooltime);
        _lock = false;
    }
}

public enum JumpPowerStage
{
    None,
    Weak,
    Normal,
    Hard
}
