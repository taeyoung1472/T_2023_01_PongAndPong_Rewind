using UnityEngine;

public class GravityInverseGimmick : GimmickObject
{
    public override void Init()
    {
    }
    Player player = null;

    GravityModule module;

    public enum GravityDirState
    {
        Up,
        Down,
    }

    public GravityDirState gravityDirState;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (module == null)
                module = other.GetComponent<GravityModule>();
            if (player == null)
                player = other.GetComponent<Player>();

            player.PlayerRenderer.flipDirection = FlipDirection.Up;
            //module.GravityScale = 0.4f;
            module.OriginGravityScale = 0.4f;

            module.GravityDir = new Vector3(0f, 9.8f, 0f);
        }
        if (other.gameObject.TryGetComponent<GravityGimmickObject>
            (out GravityGimmickObject gravityGimmick))
        {
            gravityGimmick.GravityDir = new Vector3(0, 9.8f, 0f);
            gravityGimmick.GravityScale = 0.4f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (module == null)
        {
            return;
        }
        player.PlayerRenderer.flipDirection = FlipDirection.Down;
        module.OriginGravityScale = 0.8f;
        //module.GravityScale = module.OriginGravityScale;

        module.GravityDir = new Vector3(0f, -9.8f, 0f);

        if (other.gameObject.TryGetComponent<GravityGimmickObject>
                  (out GravityGimmickObject gravityGimmick))
        {
            gravityGimmick.GravityDir = new Vector3(0, -9.8f, 0f);
            gravityGimmick.GravityScale = gravityGimmick.OrignGravityScale;
        }
    }
}
