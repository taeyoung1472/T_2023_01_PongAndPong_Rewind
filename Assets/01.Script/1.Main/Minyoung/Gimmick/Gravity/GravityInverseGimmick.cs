using UnityEngine;

public class GravityInverseGimmick : GimmickObject
{
    public override void Init()
    {
    }
    Player player = null;
    GravityModule module;
    //dlwoqud¿Ã¿Áø±

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GimmickPlayerCol"))
        {
            player.GravityModule.GravityZoneScale();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(player == null)
            player = other.GetComponentInParent<Player>();
        player.playerBuff.AddBuff(PlayerBuffType.Reverse);
    }

    private void OnTriggerExit(Collider other)
    {
        if (module == null)
        {
            return;
        }
        module.GravityScale = module.OriginGravityScale;
        player.playerBuff.DeleteBuff(PlayerBuffType.Reverse);
    }
}
