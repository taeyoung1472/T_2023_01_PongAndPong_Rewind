using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/Movement")]
public class PlayerMovementSO : ScriptableObject
{
    [Header("�̵� ����")]
    public float speed = 4f;
    [Header("���� ����")]
    public float downGravityScale = 2f;
    public float jumpContinueTime = 0.2f;
    public float jumpPower = 8f;
    public float jumpHoldPower = 3f;
    public int jumpCount = 1;
    [Header("�뽬 ����")]
    public float dashPower = 8f;
    public float dashContinueTime = 0.2f;
    public int dashCount = 1;
    [Header("�� ¤�� ����")]
    public float wallGrabJumpPower = 3f;
    public float wallSlideGravityScale = 0.5f;
    public float wallGrabJumpContinueTime = 0.12f;
}
