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

    private FlipDirection _flipDirection = FlipDirection.Down;
    public FlipDirection flipDirection
    {
        get => _flipDirection;
        set
        {
            _flipDirection = value;
            switch (_flipDirection)
            {
                case FlipDirection.None:
                    break;
                case FlipDirection.Left:
                    _player.transform.rotation = Quaternion.Euler(_player.transform.rotation.eulerAngles.x, _player.transform.rotation.eulerAngles.y, -90f);
                    break;
                case FlipDirection.Right:
                    _player.transform.rotation = Quaternion.Euler(_player.transform.rotation.eulerAngles.x, _player.transform.rotation.eulerAngles.y, 90f);
                    break;
                case FlipDirection.Up:
                    _player.transform.rotation = Quaternion.Euler(180f, _player.transform.rotation.eulerAngles.y, _player.transform.rotation.eulerAngles.z);
                    break;
                case FlipDirection.Down:
                    _player.transform.rotation = Quaternion.Euler(0f, _player.transform.rotation.eulerAngles.y, _player.transform.rotation.eulerAngles.z);
                    break;
                default:
                    break;
            }
        }
    }

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

    public void Flip(Vector2 dir)
    {
        if (dir.x == 0f)
            return;

        FlipDirection flipDir = FlipDirection.None;
        if (dir.x > 0f)
            flipDir = FlipDirection.Right;
        else if (dir.x < 0f)
            flipDir = FlipDirection.Left;

        Quaternion targetRotation = Quaternion.identity;
        if (_flipDirection == FlipDirection.Left || _flipDirection == FlipDirection.Right)
        {
            targetRotation = Quaternion.Euler((flipDir == FlipDirection.Left) ? -90f : 90f, _player.transform.rotation.eulerAngles.y, _player.transform.rotation.eulerAngles.z); //플레이어 로테이션을 돌린다 
        }
        else
        {
            targetRotation = Quaternion.Euler(_player.transform.rotation.eulerAngles.x, (flipDir == FlipDirection.Left) ? -90f : 90f, _player.transform.rotation.eulerAngles.z); //플레이어 로테이션을 돌린다 
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
        _fliped = flipDir == FlipDirection.Left;
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


}

public enum FlipDirection
{
    None,
    Left,
    Right,
    Up,
    Down
}
