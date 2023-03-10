using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/Movement")]
public class PlayerMovementSO : ScriptableObject
{
    [Header("�̵� ����")]
    public float speed = 4f;
    [Header("���� ����")]
    public float fallMultiplier = 2.5f;
    public float downGravityScale = 2f;
    public float jumpPower = 8f;
    public float jumpHoldTime = 0.5f;
    public int jumpCount = 1;
    [Header("�뽬 ����")]
    public float dashPower = 8f;
    public float dashContinueTime = 0.2f;
    public float dashChargeTime = 0.2f;
    public int dashCount = 1;
    [Header("�� ¤�� ����")]
    public Vector2 wallJumpPower = Vector2.zero;
    public float wallGrabJumpPower = 3f;
    public float wallSlideGravityScale = 0.5f;
    public float moveLockTime = 0.2f;
}
