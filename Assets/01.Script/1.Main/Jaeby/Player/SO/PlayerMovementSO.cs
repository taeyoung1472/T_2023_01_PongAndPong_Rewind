using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/Movement")]
public class PlayerMovementSO : ScriptableObject
{
    [Header("이동 관련")]
    public float speed = 4f;
    [Header("점프 관련")]
    public float downGravityScale = 2f;
    public float jumpContinueTime = 0.2f;
    public float jumpPower = 8f;
    public float jumpHoldPower = 3f;
    public int jumpCount = 1;
    [Header("대쉬 관련")]
    public float dashPower = 8f;
    public float dashContinueTime = 0.2f;
    public int dashCount = 1;
    [Header("벽 짚기 관련")]
    public float wallGrabJumpPower = 3f;
    public float wallSlideGravityScale = 0.5f;
    public float wallGrabJumpContinueTime = 0.12f;
}
