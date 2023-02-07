using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMove : PlayerAction
{
    private bool _fliped = false;
    public bool Fliped => _fliped;
    [SerializeField]
    private float _rotationSpeed = 1000f;
    [SerializeField]
    private UnityEvent<bool> OnFliped = null;

    private void Update()
    {
        if (_locked)
            return;
        Move(_player.PlayerInput.InputVectorNorm);
        Flip(_player.PlayerInput.InputVectorNorm);
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

    public void Flip(Vector2 dir)
    {
        if (dir.x == 0f)
            return;

        FlipDirection flipDir = FlipDirection.None;
        if (dir.x > 0f)
            flipDir = FlipDirection.Right;
        else if (dir.x < 0f)
            flipDir = FlipDirection.Left;

        Quaternion targetRotation = Quaternion.Euler(0f, (flipDir == FlipDirection.Left) ? -90f : 90f, 0f);
        //Debug.Log($"x : {targetRotation.eulerAngles.x} y : {targetRotation.eulerAngles.y} z : {targetRotation.eulerAngles.z}");
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        _fliped = flipDir == FlipDirection.Left;
        OnFliped?.Invoke(_fliped);
    }

    private Vector2 GetDirToVector(FlipDirection dir)
    {
        switch (dir)
        {
            case FlipDirection.None:
                break;
            case FlipDirection.Left:
                return Vector2.left;
            case FlipDirection.Right:
                return Vector2.right;
            case FlipDirection.Up:
                return Vector2.up;
            case FlipDirection.Down:
                return Vector2.down;
            default:
                break;
        }
        return Vector2.zero;
    }

    enum FlipDirection
    {
        None,
        Left,
        Right,
        Up,
        Down
    }
}
