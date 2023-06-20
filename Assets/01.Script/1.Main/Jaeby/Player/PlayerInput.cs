using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<Vector2> OnMoveInput = null;
    [SerializeField]
    private UnityEvent OnJumpStart = null;
    [SerializeField]
    private UnityEvent OnJumpEnd = null;
    [SerializeField]
    private UnityEvent OnDash = null;
    [SerializeField]
    private UnityEvent OnAttack = null;
    [SerializeField]
    private UnityEvent OnWeaponChange = null;
    [SerializeField]
    private UnityEvent OnInteract = null;

    private Vector2 _inputVector = Vector2.zero;
    public Vector2 InputVectorNorm => _inputVector.normalized;
    public Vector2 InputVector => _inputVector;
    public Vector2 RotatedInputVector => Quaternion.FromToRotation(Vector2.up, Utility.GetDirToVector(_player.PlayerRenderer.flipDirection)) * _inputVector;

    private Player _player = null;

    private void Awake()
    {
        _player = GetComponent<Player>();
        KeyManager.LoadKey();

        PlayerInteract playerInteract = _player.GetPlayerAction<PlayerInteract>(PlayerActionType.Interact);
        if ((playerInteract?.TryInteract()).Value)
            OnInteract?.Invoke();
    }

    private void Update()
    {
        if (_player == null)
            return;
        if (Input.GetKeyDown(KeyManager.keys[InputType.Interact]))
        {
            PlayerInteract playerInteract = _player.GetPlayerAction<PlayerInteract>(PlayerActionType.Interact);
            if (playerInteract == null)
                return;

            if (playerInteract.TryInteract())
            {
                OnInteract?.Invoke();
                return;
            }
        }

        int x = 0, y = 0;
        if (Input.GetKey(KeyManager.keys[InputType.Right]))
            x++;
        if (Input.GetKey(KeyManager.keys[InputType.Left]))
            x--;
        if (_player.playerBuff.BuffCheck(PlayerBuffType.Reverse) || !_player.PlayerRenderer.GetHorizontalFlip())
        {
            x *= -1;
        }
        _inputVector = new Vector2(x, y);
        OnMoveInput?.Invoke(new Vector2(x, y));

        if (Input.GetKeyDown(KeyManager.keys[InputType.Jump]))
            OnJumpStart?.Invoke();
        if (Input.GetKeyUp(KeyManager.keys[InputType.Jump]))
            OnJumpEnd?.Invoke();
        if (Input.GetKeyDown(KeyManager.keys[InputType.Dash]))
            OnDash?.Invoke();
        if (Input.GetKeyDown(KeyManager.keys[InputType.Attack]))
            OnAttack?.Invoke();
        if (Input.GetKeyDown(KeyManager.keys[InputType.WeaponChange]))
            OnWeaponChange?.Invoke();
    }

    public void InputVectorReset()
    {
        _inputVector = Vector2.zero;
        OnMoveInput?.Invoke(_inputVector);
    }
}
