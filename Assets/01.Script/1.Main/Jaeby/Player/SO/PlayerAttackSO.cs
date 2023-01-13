using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/Attack")]
public class PlayerAttackSO : ScriptableObject
{
    public float weaponSwitchingDelay = 0.2f;
    public float attackAnimationDelayNomalizeTime = 0.4f;
    public float meleeAttackDelay = 
        .1f;
    public float rangeAttackDelay = 0.1f;
    public int meleeAttackPower = 1;
    public int rangeAttackPower = 1;
    public float bulletSpeed = 3f;
    public GameObject bulletPrefab = null;
}
