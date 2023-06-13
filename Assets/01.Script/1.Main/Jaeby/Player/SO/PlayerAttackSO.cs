using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/Attack")]
public class PlayerAttackSO : ScriptableObject
{
    public float weaponSwitchingDelay = 0.2f;
    public float meleeAttackDelay = 
        .1f;
    public float rangeAttackDelay = 0.1f;
    public int meleeAttackPower = 1;
    public int rangeAttackPower = 1;
    public float bulletSpeed = 3f;

    [Space(20)]
    public float meleeAttackResetTime = 0.5f;
    public float flipLockTime = 0.4f;
    public float ikEndAnimationTime = 0.2f;
    public float ikCancelMaxAngle = 30f;
}
