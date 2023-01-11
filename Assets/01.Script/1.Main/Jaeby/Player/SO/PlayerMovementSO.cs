using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/Movement")]
public class PlayerMovementSO : ScriptableObject
{
    [Header("�̵� ����")]
    public float speed = 4f;
    [Header("���� ����")]
    public float jumpContinueTime = 0.2f;
    public float jumpPower = 8f;
    public int jumpCount = 1;
    [Header("�뽬 ����")]
    public float dashPower = 8f;
    public float dashContinueTime = 0.2f;
    public int dashCount = 1;
    [Header("�� ¤�� ����")]
    public float aaa;
}
