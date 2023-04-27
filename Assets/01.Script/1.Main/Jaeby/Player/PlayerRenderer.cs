using UnityEngine;
using UnityEngine.Events;

public class PlayerRenderer : MonoBehaviour
{
    private bool _fliped = false;
    public bool Fliped => _fliped;

    [SerializeField]
    private UnityEvent<bool> OnFliped = null;

    private Player _player = null;

    public Vector3 Forward => transform.forward;
    public Vector3 Down => -transform.up;

    private DirectionType _flipDirection = DirectionType.Down;
    public DirectionType flipDirection
    {
        get => _flipDirection;
        set
        {
            _flipDirection = value;
            switch (_flipDirection)
            {
                case DirectionType.None:
                    break;
                case DirectionType.Left:
                    _player.transform.rotation = Quaternion.Euler(_player.transform.rotation.eulerAngles.y, 180f, 90f);
                    break;
                case DirectionType.Right:
                    _player.transform.rotation = Quaternion.Euler(_player.transform.rotation.eulerAngles.y, 0f, 90f);
                    break;
                case DirectionType.Up:
                    _player.transform.rotation = Quaternion.Euler(180f, _player.transform.rotation.eulerAngles.y, 0f);
                    break;
                case DirectionType.Down:
                    _player.transform.rotation = Quaternion.Euler(0f, _player.transform.rotation.eulerAngles.y, 0f);
                    break;
                default:
                    break;
            }
        }
    }

    private void Awake()
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

    public void Flip(Vector2 dir)
    {
        if (dir.x == 0f)
            return;

        DirectionType flipDir = DirectionType.None;
        if (dir.x > 0f)
            flipDir = DirectionType.Right;
        else if (dir.x < 0f)
            flipDir = DirectionType.Left;

        Quaternion targetRotation = Quaternion.identity;
        if (_flipDirection == DirectionType.Left)
        {
            targetRotation = Quaternion.Euler((flipDir == DirectionType.Left) ? -90f : 90f, 180f, 90f); //플레이어 로테이션을 돌린다 
        }
        else if (_flipDirection == DirectionType.Right)
        {
            targetRotation = Quaternion.Euler((flipDir == DirectionType.Left) ? -90f : 90f, 0f, 90f); //플레이어 로테이션을 돌린다 
        }
        else
        {
            targetRotation = Quaternion.Euler(_player.transform.rotation.eulerAngles.x, (flipDir == DirectionType.Left) ? -90f : 90f, _player.transform.rotation.eulerAngles.z); //플레이어 로테이션을 돌린다 
        }
        _player.transform.rotation = targetRotation;
        //_player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        /*Vector3 sc = transform.localScale;
        sc.x = Mathf.Abs(sc.x);
        if (flipDir == FlipDirection.Left)
        {
            sc.x *= -1f;
        }
        transform.localScale = sc;*/
        _fliped = flipDir == DirectionType.Left;
        OnFliped?.Invoke(_fliped);
    }

    public void ForceFlip()
    {
        // left : -90 right : 90
        Quaternion targetRotation = Quaternion.Euler(_player.transform.rotation.eulerAngles.x, _fliped ? 90f : -90f, _player.transform.rotation.eulerAngles.z); // 반대
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



}
