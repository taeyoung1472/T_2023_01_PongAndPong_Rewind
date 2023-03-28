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
            player = other.GetComponentInParent<Player>();
            player.GravityModule.GravityZoneScale();
        }
    }
   
    private void OnTriggerExit(Collider other)
    {
        if (module == null)
        {
            return;
        }
        module.GravityScale = module.OriginGravityScale;
    }
}
