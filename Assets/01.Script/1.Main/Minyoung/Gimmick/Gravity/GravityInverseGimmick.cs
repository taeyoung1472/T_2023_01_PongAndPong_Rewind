using UnityEngine;

public class GravityInverseGimmick : GimmickObject
{
    public override void Init()
    {
    }
    Player player = null;
    GravityModule module;
    [SerializeField]
    private float gravityZoneScale = 0.2f;
    //dlwoqud¿Ã¿Áø±

    private void OnTriggerEnter(Collider other)
    {
        if(player == null)
            player = other.GetComponentInParent<Player>();
        if (player == null)
            return;

        player.playerBuff.AddBuff(PlayerBuffType.Reverse);
        player.GravityModule.GravityScale = gravityZoneScale;
    }

    private void OnTriggerExit(Collider other)
    {
        if (player == null)
            return;
        player.playerBuff.DeleteBuff(PlayerBuffType.Reverse);
        player.GravityModule.GravityScale = player.GravityModule.OriginGravityScale;
    }
}
