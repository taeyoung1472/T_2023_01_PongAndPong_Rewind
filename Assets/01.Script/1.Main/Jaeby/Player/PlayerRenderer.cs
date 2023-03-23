using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerRenderer : MonoBehaviour
{
    private bool _fliped = false;
    public bool Fliped => _fliped;
    [SerializeField]
    private float _rotationSpeed = 1000f;
    [SerializeField]
    private UnityEvent<bool> OnFliped = null;

    private Player _player = null;

    public Vector3 Forward => transform.forward;

    private void Start()
    {
        _player = GetComponentInParent<Player>();
    }

    public Quaternion GetFlipedRotation(DirType dirType, RotAxis rotAxis)
    {
        //오른쪽 기준
        Quaternion rot = _player.transform.rotation; // left = -90
        switch (rotAxis)
        {
            case RotAxis.None:
                break;
            case RotAxis.X:
                rot = Quaternion.Euler(rot.eulerAngles.y, 0f, 0f);
                break;
            case RotAxis.Y:
                rot = Quaternion.Euler(0f, rot.eulerAngles.y, 0f);
                break;
            case RotAxis.Z:
                rot = Quaternion.Euler(0f, 0f, rot.eulerAngles.y);
                break;
            default:
                break;
        }

        if (dirType == DirType.Back)
            rot = Quaternion.Inverse(rot);

        return rot;
    }

    private void Update()
    {
        if(_player.PlayerActionLockCheck(PlayerActionType.Move) == false)
        {
            Flip(_player.PlayerInput.InputVectorNorm);
        }
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
        _player.transform.rotation = targetRotation;
        //_player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        /*Vector3 sc = transform.localScale;
        sc.x = Mathf.Abs(sc.x);
        if (flipDir == FlipDirection.Left)
        {
            sc.x *= -1f;
        }
        transform.localScale = sc;*/
        _fliped = flipDir == FlipDirection.Left;
        OnFliped?.Invoke(_fliped);
    }

    public void ForceFlip()
    {
        // left : -90 right : 90
        Quaternion targetRotation = Quaternion.Euler(0f, _fliped ? 90f : -90f, 0f); // 반대
        _player.transform.rotation = targetRotation;
        /*Vector3 sc = transform.localScale;
        sc.x = Mathf.Abs(sc.x);
        if (_fliped == false)
        {
            sc.x *= -1f;
        }
        transform.localScale = sc;*/
        _fliped = !_fliped;
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

public enum DirType
{
    None,
    Forward,
    Back
}

public enum RotAxis
{
    None,
    X,
    Y,
    Z
}
